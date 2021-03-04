using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Exante.Net.Converters;
using Exante.Net.Enums;
using Exante.Net.Interfaces;
using Exante.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private const string ordersEndpoint = "stream/orders";
        private const string tradesEndpoint = "stream/trades";

        private const string apiVersion = "3.0";
        
        private const string dataEndpointType = "md";
        private const string tradeEndpointType = "trade";

        private const string eventType = "event";
        private const string eventTypeOrder = "order";
        private const string eventTypeTrade = "trade";
        private const string eventTypeHeartbeat = "heartbeat";
        private const string eventOrderDataPath = "order";
        
        #endregion

        #region Fields

        private readonly TimeSpan reconnectStreamTimeout;
        
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
            reconnectStreamTimeout = options.ReconnectStreamTimeout;
        }
        
        public void SetApiCredentials(string clientId, string applicationId, string sharedKey)
        {
            SetAuthenticationProvider(new ExanteAuthenticationProvider(new ExanteApiCredentials(clientId, applicationId, sharedKey)));
        }
        
        #endregion
        
        #region Live feed API

        /// <summary>
        /// Get live feed trades stream
        /// </summary>
        /// <returns>Trades stream for the specified financial instrument</returns>
        public async Task<WebCallResult<ExanteStreamSubscription>> GetFeedTradesStreamAsync(IEnumerable<string> symbolIds, 
            Action<ExanteFeedTrade> onNewTrade, CancellationToken ct = default)
        {
            if (symbolIds == null)
                throw new ArgumentException("Symbol(s) must be sent");
            
            var symbols = symbolIds.ToArray();
            
            if (!symbols.Any())
                throw new ArgumentException("Symbol(s) must be sent");

            if (symbols.Any(string.IsNullOrEmpty))
                throw new ArgumentException("Symbol can't be empty");
            
            var url = GetUrl(feedTradesEndpoint, dataEndpointType, apiVersion, string.Join(",", symbols.Distinct()));

            var result = await CreateStreamAsync(url, ct, null, x =>
            {
                log.Write(LogVerbosity.Debug, x);
                
                var data = Deserialize<ExanteFeedTrade>(x, false);
                if (data.Success)
                {
                    if (!data.Data.IsEmpty)
                        onNewTrade(data.Data);
                }
                else
                    log.Write(LogVerbosity.Warning, "Couldn't deserialize data received from stream: " + data.Error);

            }).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get live feed quote stream
        /// </summary>
        /// <returns>Life quote stream for the specified financial instrument</returns>
        public async Task<WebCallResult<ExanteStreamSubscription>> GetFeedQuoteStreamAsync(IEnumerable<string> symbolIds, 
            Action<ExanteTickShort> onNewQuote, ExanteQuoteLevel level = ExanteQuoteLevel.BestPrice, CancellationToken ct = default)
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

            var url = GetUrl(feedEndpoint, dataEndpointType, apiVersion, string.Join(",", symbols.Distinct()));

            var result = await CreateStreamAsync(url, ct, parameters, x =>
            {
                log.Write(LogVerbosity.Debug, x);
                
                var data = Deserialize<ExanteTickShort>(x, false);
                if (data.Success)
                {
                    if (!data.Data.IsEmpty)
                        onNewQuote(data.Data);
                }
                else
                    log.Write(LogVerbosity.Warning, "Couldn't deserialize data received from stream: " + data.Error);

            }).ConfigureAwait(false);

            return result;
        }
        
        #endregion

        #region Orders stream API
        
        /// <summary>
        /// Get order updates stream
        /// </summary>
        public async Task<WebCallResult<ExanteStreamSubscription>> GetOrdersStreamAsync(Action<ExanteOrder> onNewOrder, CancellationToken ct = default)
        {
            var url = GetUrl(ordersEndpoint, tradeEndpointType, apiVersion);

            var result = await CreateStreamAsync(url, ct, null, x =>
            {
                log.Write(LogVerbosity.Debug, x);
                
                var token = JToken.Parse(x);
                
                var type = (string?)token[eventType];
                if (type == null)
                    return;

                switch (type)
                {
                    case eventTypeOrder:
                    {
                        var order = token[eventOrderDataPath];
                        if (order == null)
                            return;
                        
                        var data = Deserialize<ExanteOrder>(order, false);
                        if (data.Success)
                        {
                            if (!data.Data.IsEmpty)
                                onNewOrder(data.Data);
                        }
                        else
                            log.Write(LogVerbosity.Warning, "Couldn't deserialize data received from stream: " + data.Error);

                        break;
                    }
                    case eventTypeHeartbeat:
                    {
                        log.Write(LogVerbosity.Debug, "Heartbeat event received");
                        break;
                    }
                }
            }).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Trades stream API
        
        /// <summary>
        /// Get trades stream
        /// </summary>
        public async Task<WebCallResult<ExanteStreamSubscription>> GetTradesStreamAsync(Action<ExanteTrade> onNewTrade, CancellationToken ct = default)
        {
            var url = GetUrl(tradesEndpoint, tradeEndpointType, apiVersion);

            var result = await CreateStreamAsync(url, ct, null, x =>
            {
                log.Write(LogVerbosity.Debug, x);

                var token = JToken.Parse(x);

                var type = (string?)token[eventType];
                if (type == null)
                    return;

                switch (type)
                {
                    case eventTypeTrade:
                    {
                        var data = Deserialize<ExanteTrade>(token, false);
                        if (data.Success)
                        {
                            if (!data.Data.IsEmpty)
                                onNewTrade(data.Data);
                        }
                        else
                            log.Write(LogVerbosity.Warning, "Couldn't deserialize data received from stream: " + data.Error);

                        break;
                    }
                    case eventTypeHeartbeat:
                    {
                        log.Write(LogVerbosity.Debug, "Heartbeat event received");
                        break;
                    }
                }

            }).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Stream common

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
            var streamSubscription = new ExanteStreamSubscription(Guid.NewGuid());

            var streamData = await GetStreamAsync(CreateRequest(uri, parameters), cancellationToken).ConfigureAwait(true);
            if (!streamData.Success)
                return new WebCallResult<ExanteStreamSubscription>(streamData.ResponseStatusCode,
                    streamData.ResponseHeaders, default, streamData.Error);

            StartSubscription(streamSubscription.Id, uri, parameters, action, streamData.Data);

            return new WebCallResult<ExanteStreamSubscription>(streamData.ResponseStatusCode, 
                streamData.ResponseHeaders, streamSubscription, default);
        }

        private async Task ReCreateStreamAsync(ExanteStream streamData, Action<string> action)
        {
            while (streamSubscriptions.ContainsKey(streamData.Id))
            {
                try
                {
                    log.Write(LogVerbosity.Debug, $"[{streamData.Id}] Try reconnect to {streamData.Uri}");
                    
                    var newStreamData = await GetStreamAsync(CreateRequest(streamData.Uri, streamData.Parameters), new CancellationToken()).ConfigureAwait(true);
                    if (!newStreamData.Success)
                    {
                        await Task.Delay(reconnectStreamTimeout).ConfigureAwait(true);
                        continue;
                    }

                    StartSubscription(streamData.Id, streamData.Uri, streamData.Parameters, action, newStreamData.Data);
                    break;
                }
                catch (Exception e)
                {
                    log.Write(LogVerbosity.Debug, $"[{streamData.Id}] Reconnect error: {e.Message}");
                    await Task.Delay(reconnectStreamTimeout).ConfigureAwait(true);
                }
            }
        }

        private void StartSubscription(Guid subscriptionId, Uri uri, Dictionary<string, object>? parameters, 
            Action<string> action, ExanteStream streamData)
        {
            var readTask = new Task(async () => await ReadFromStreamAsync(streamData, action.Invoke).ConfigureAwait(true));

            streamData.Id = subscriptionId;
            streamData.Uri = uri;
            streamData.Parameters = parameters;
            streamData.StreamTask = readTask;

            streamSubscriptions[subscriptionId] = streamData;
            
            readTask.Start();
        }

        private async Task ReadFromStreamAsync(ExanteStream streamData, Action<string> action)
        {
            try
            {
                using var streamReader = new StreamReader(streamData.ResponseStream);

                while (!streamReader.EndOfStream)
                {
                    var response = await streamReader.ReadLineAsync().ConfigureAwait(false);
                    
                    if ((response?.Length ?? 0) > 0)
                        action.Invoke(response!);
                }
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Debug, $"[{streamData.Id}] Stream error received: {e.Message}");
            }

            streamData.ResponseStream.Close();
            streamData.Response.Close();

            if (!streamSubscriptions.ContainsKey(streamData.Id))
                return;

            await ReCreateStreamAsync(streamData, action);
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