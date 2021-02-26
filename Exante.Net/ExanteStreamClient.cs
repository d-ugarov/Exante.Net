using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Exante.Net.Converters;
using Exante.Net.Enums;
using Exante.Net.Interfaces;
using Exante.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Exante.Net
{
    public class ExanteStreamClient : RestClient, IExanteStreamClient
    {
        #region Endpoints
        
        private const string feedEndpoint = "feed";
        private const string feedTradesEndpoint = "feed/trades";
        private const string tradesEndpoint = "trades";

        private const string apiVersion = "3.0";
        
        private const string dataEndpointType = "md";
        private const string tradeEndpointType = "trade";
        
        #endregion

        #region Fields

        private readonly ConcurrentDictionary<Guid, ExanteStream> streamSubscriptions = new();

        #endregion

        #region Constructors

        public ExanteStreamClient(string clientId, string applicationId, string sharedKey)
            : this(new ExanteClientOptions(new ExanteApiCredentials(clientId, applicationId, sharedKey)))
        {
        }

        public ExanteStreamClient(ExanteApiCredentials credentials) 
            : this(new ExanteClientOptions(credentials))
        {
        }

        public ExanteStreamClient(ExanteClientOptions options)
            : base("Exante", options, options.ExanteApiCredentials == null ? null : new ExanteAuthenticationProvider(options.ExanteApiCredentials))
        {
        }
        
        public void SetApiCredentials(string clientId, string applicationId, string sharedKey)
        {
            SetAuthenticationProvider(new ExanteAuthenticationProvider(new ExanteApiCredentials(clientId, applicationId, sharedKey)));
        }
        
        #endregion

        #region Stream API

        public async Task<WebCallResult<ExanteStreamSubscription>> GetQuoteStreamAsync(IEnumerable<string> symbolIds, 
            Action<string> onNewQuote, ExanteQuoteLevel level = ExanteQuoteLevel.BestPrice, CancellationToken ct = default)
        {
            if (symbolIds == null)
                throw new ArgumentException("Symbol(s) must be sent");
            
            var symbols = symbolIds.ToArray();
            
            if (!symbols.Any())
                throw new ArgumentException("Symbol(s) must be sent");

            if (symbols.Any(string.IsNullOrEmpty))
                throw new ArgumentException("Symbol can't be empty");

            var parameters = new Dictionary<string, object>
                             {
                                 {"level", JsonConvert.SerializeObject(level, new QuoteLevelConverter(false))},
                             };

            var url = GetUrl(feedEndpoint, dataEndpointType, apiVersion, string.Join(",", symbols));

            var result = await CreateStreamAsync(url, ct, parameters, x =>
            {
                onNewQuote.Invoke(x);

            }).ConfigureAwait(false);

            return result;
        }

        public CallResult StopStream(ExanteStreamSubscription subscription)
        {
            if (!streamSubscriptions.TryRemove(subscription.Id, out var value))
                return new CallResult(new ArgumentError("Wrong stream subscription"));

            value.ResponseStream.Close();
            value.Response.Close();

            return new CallResult(null);
        }
        
        #endregion

        #region Streams

        private async Task<WebCallResult<ExanteStreamSubscription>> CreateStreamAsync(Uri uri, 
            CancellationToken cancellationToken, Dictionary<string, object>? parameters, Action<string> action)
        {
            var request = CreateRequest(uri, parameters);

            var streamSubscription = new ExanteStreamSubscription(Guid.NewGuid());

            var stream = await GetStreamAsync(request, cancellationToken);
            if (!stream.Success)
                return new WebCallResult<ExanteStreamSubscription>(stream.ResponseStatusCode,
                    stream.ResponseHeaders, default, stream.Error);

            var task = new Task(async () => await ReadFromStream(stream.Data, action.Invoke));

            stream.Data.Uri = uri;
            stream.Data.Parameters = parameters;
            stream.Data.Id = streamSubscription.Id;
            stream.Data.StreamTask = task;

            streamSubscriptions[streamSubscription.Id] = stream.Data;
            task.Start();
                
            return new WebCallResult<ExanteStreamSubscription>(stream.ResponseStatusCode, 
                stream.ResponseHeaders, streamSubscription, default);
        }

        private async Task ReadFromStream(ExanteStream stream, Action<string> action)
        {
            while (true)
            {
                try
                {
                    using var streamReader = new StreamReader(stream.ResponseStream);

                    while (!streamReader.EndOfStream)
                    {
                        var response = await streamReader.ReadLineAsync().ConfigureAwait(false);
                        action.Invoke(response);
                    }
                }
                catch (Exception e)
                {
                    log.Write(LogVerbosity.Debug, $"[{stream.Id}] Stream error received: {e.Message}");
                }

                if (!streamSubscriptions.ContainsKey(stream.Id))
                    return;

                // todo: reconnect
            }
        }

        private async Task<WebCallResult<ExanteStream>> GetStreamAsync(IRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await request.GetResponse(cancellationToken).ConfigureAwait(false);
                var statusCode = response.StatusCode;
                var headers = response.ResponseHeaders;
                var responseStream = await response.GetResponseStream().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = new ExanteStream(response, responseStream);
                    
                    return new WebCallResult<ExanteStream>(statusCode, headers, result, default);
                }
                else
                {
                    using var reader = new StreamReader(responseStream);
                    var data = await reader.ReadToEndAsync().ConfigureAwait(false);
                    log.Write(LogVerbosity.Debug, $"[{request.RequestId}] Error received: {data}");
                    responseStream.Close();
                    response.Close();
                    var parseResult = ValidateJson(data);
                    var error = parseResult.Success ? ParseErrorResponse(parseResult.Data) : parseResult.Error!;
                    if(error.Code == null || error.Code == 0)
                        error.Code = (int)response.StatusCode;
                    return new WebCallResult<ExanteStream>(statusCode, headers, default, error);
                }
            }
            catch (HttpRequestException requestException)
            {
                var exceptionInfo = GetExceptionInfo(requestException);
                log.Write(LogVerbosity.Warning, $"[{request.RequestId}] Request exception: " + exceptionInfo);
                return new WebCallResult<ExanteStream>(null, null, default, new WebError(exceptionInfo));
            }
            catch (TaskCanceledException canceledException)
            {
                if (canceledException.CancellationToken == cancellationToken)
                {
                    // Cancellation token cancelled
                    log.Write(LogVerbosity.Warning, $"[{request.RequestId}] Request cancel requested");
                    return new WebCallResult<ExanteStream>(null, null, default, new CancellationRequestedError());
                }
                else
                {
                    // Request timed out
                    log.Write(LogVerbosity.Warning, $"[{request.RequestId}] Request timed out");
                    return new WebCallResult<ExanteStream>(null, null, default, new WebError($"[{request.RequestId}] Request timed out"));
                }
            }
        }

        private IRequest CreateRequest(Uri uri, Dictionary<string, object>? parameters = null) 
        {
            var requestId = NextId();
            log.Write(LogVerbosity.Debug, $"[{requestId}] Creating request for " + uri);
            if (authProvider == null)
            {
                log.Write(LogVerbosity.Warning, $"[{requestId}] Request {uri.AbsolutePath} failed because no ApiCredentials were provided");
                throw new Exception($"Request {uri.AbsolutePath} failed because no ApiCredentials were provided");
            }

            parameters ??= new Dictionary<string, object>();

            var uriString = uri.ToString();

            if (parameters.Any())
                uriString += "?" + parameters.CreateParamString(true, arraySerialization);

            var request = RequestFactory.Create(HttpMethod.Get, uriString, requestId);
            request.Accept = "application/x-json-stream";
            
            foreach (var data in authProvider.AddAuthenticationToHeaders(uriString, HttpMethod.Get, parameters, true, default, arraySerialization))
            {
                request.AddHeader(data.Key, data.Value);
            }

            return request;
        }

        #endregion

        #region Helpers
        
        private Uri GetUrl(string endpoint, string endpointType, string version, string? optionalPath = null)
        {
            var result = $"{BaseAddress}{endpointType}/{version}/{endpoint}";

            if (!string.IsNullOrEmpty(optionalPath))
                result += $"/{optionalPath}";
            
            return new Uri(result);
        }

        #endregion
    }
}