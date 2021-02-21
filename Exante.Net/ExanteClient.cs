using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Exante.Net.Interfaces;
using Exante.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Exante.Net
{
    public class ExanteClient : RestClient, IExanteClient
    {
        #region Endpoints

        private const string exchangesEndpoint = "exchanges";

        private const string apiVersion = "3.0";
        
        private const string dataEndpointType = "md";
        private const string tradeEndpointType = "trade";

        #endregion

        #region Constructors

        public ExanteClient(string clientId, string applicationId, string sharedKey)
            : this(new ExanteClientOptions(new ExanteApiCredentials(clientId, applicationId, sharedKey)))
        {
        }

        public ExanteClient(ExanteApiCredentials credentials) 
            : this(new ExanteClientOptions(credentials))
        {
        }

        public ExanteClient(ExanteClientOptions options)
            : base("Exante", options, options.ExanteApiCredentials == null ? null : new ExanteAuthenticationProvider(options.ExanteApiCredentials))
        {
            arraySerialization = ArrayParametersSerialization.MultipleValues;
            postParametersPosition = PostParameters.InBody;
            requestBodyFormat = RequestBodyFormat.FormData;
            requestBodyEmptyContent = "";
        }
        
        public void SetApiCredentials(string clientId, string applicationId, string sharedKey)
        {
            SetAuthenticationProvider(new ExanteAuthenticationProvider(new ExanteApiCredentials(clientId, applicationId, sharedKey)));
        }
        
        #endregion

        #region Symbols API

        /// <summary>
        /// Get exchanges
        /// </summary>
        public async Task<WebCallResult<IEnumerable<ExanteExchange>>> GetExchangesAsync(CancellationToken ct = default)
        {
            var url = GetUrl(exchangesEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteExchange>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get instruments by exchange
        /// </summary>
        public async Task<WebCallResult<IEnumerable<ExanteSymbolInfo>>> GetExchangeInstrumentsAsync(string exchangeId, CancellationToken ct = default)
        {
            exchangeId.ValidateNotNull(nameof(exchangeId));
            
            var url = GetUrl(exchangesEndpoint, dataEndpointType, apiVersion, exchangeId);
            return await SendRequest<IEnumerable<ExanteSymbolInfo>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
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