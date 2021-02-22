using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Exante.Net.Converters;
using Exante.Net.Enums;
using Exante.Net.Interfaces;
using Exante.Net.Objects;
using Newtonsoft.Json;
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

        private const string accountsEndpoint = "accounts";
        private const string changesEndpoint = "change";
        private const string crossRatesEndpoint = "crossrates";
        private const string crossRatesFromToEndpoint = "crossrates/{0}/{1}";
        private const string exchangesEndpoint = "exchanges";
        private const string groupsEndpoint = "groups";
        private const string groupsNearestEndpoint = "groups/{0}/nearest";
        private const string symbolsEndpoint = "symbols";
        private const string symbolsScheduleEndpoint = "symbols/{0}/schedule";
        private const string symbolsSpecificationEndpoint = "symbols/{0}/specification";
        private const string typesEndpoint = "types";

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

        #region Accounts API
        
        /// <summary>
        /// Get user accounts
        /// </summary>
        /// <returns>List of user accounts and their statuses</returns>
        public async Task<WebCallResult<IEnumerable<ExanteAccount>>> GetAccountsAsync(CancellationToken ct = default)
        {
            var url = GetUrl(accountsEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteAccount>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        #endregion

        #region Daily change API
        
        /// <summary>
        /// Get daily changes
        /// <remarks>Returns all symbols daily changes if symbols is empty</remarks>
        /// </summary>
        /// <returns>List of daily changes</returns>
        public async Task<WebCallResult<IEnumerable<ExanteDailyChange>>> GetDailyChangesAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
        {
            var symbolsPath = symbols != null ? string.Join(",", symbols) : null;
            var url = GetUrl(changesEndpoint, dataEndpointType, apiVersion, symbolsPath);
            return await SendRequest<IEnumerable<ExanteDailyChange>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        #endregion

        #region Crossrates API
        
        /// <summary>
        /// Get list of available currencies
        /// </summary>
        /// <returns>List of available currencies</returns>
        public async Task<WebCallResult<ExanteAvailableCrossRates>> GetAvailableCurrenciesAsync(CancellationToken ct = default)
        {
            var url = GetUrl(crossRatesEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<ExanteAvailableCrossRates>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get cross rate
        /// </summary>
        /// <returns>Cross rate from one currency to another</returns>
        public async Task<WebCallResult<ExanteCrossRate>> GetCrossRateAsync(string from, string to, CancellationToken ct = default)
        {
            from.ValidateNotNull(nameof(from));
            to.ValidateNotNull(nameof(to));
            
            var url = GetUrl(string.Format(crossRatesFromToEndpoint, from.ToUpperInvariant(), to.ToUpperInvariant()), dataEndpointType, apiVersion);
            return await SendRequest<ExanteCrossRate>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        #endregion

        #region Symbols API
        
        /// <summary>
        /// Get exchanges
        /// </summary>
        /// <returns>List of exchanges</returns>
        public async Task<WebCallResult<IEnumerable<ExanteExchange>>> GetExchangesAsync(CancellationToken ct = default)
        {
            var url = GetUrl(exchangesEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteExchange>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get instruments by exchange
        /// </summary>
        /// <returns>Exchange financial instruments</returns>
        public async Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsByExchangeAsync(string exchangeId, CancellationToken ct = default)
        {
            exchangeId.ValidateNotNull(nameof(exchangeId));
            
            var url = GetUrl(exchangesEndpoint, dataEndpointType, apiVersion, exchangeId);
            return await SendRequest<IEnumerable<ExanteSymbol>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instrument groups
        /// </summary>
        /// <returns>Available instrument groups</returns>
        public async Task<WebCallResult<IEnumerable<ExanteInstrumentGroup>>> GetSymbolAllGroupsAsync(CancellationToken ct = default)
        {
            var url = GetUrl(groupsEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteInstrumentGroup>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instruments by group
        /// </summary>
        /// <returns>Financial instruments which belong to specified group</returns>
        public async Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsByGroupAsync(string groupId, CancellationToken ct = default)
        {
            groupId.ValidateNotNull(nameof(groupId));
            
            var url = GetUrl(groupsEndpoint, dataEndpointType, apiVersion, groupId);
            return await SendRequest<IEnumerable<ExanteSymbol>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get nearest expiration in group
        /// </summary>
        /// <returns>Financial instrument which has the nearest expiration in the group</returns>
        public async Task<WebCallResult<ExanteSymbol>> GetSymbolNearestExpirationInGroupAsync(string groupId, CancellationToken ct = default)
        {
            groupId.ValidateNotNull(nameof(groupId));

            var url = GetUrl(string.Format(groupsNearestEndpoint, groupId), dataEndpointType, apiVersion);
            return await SendRequest<ExanteSymbol>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instrument list
        /// </summary>
        /// <returns>List of instruments available for authorized user</returns>
        public async Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var url = GetUrl(symbolsEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteSymbol>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instrument
        /// </summary>
        /// <returns>Instrument available for authorized user</returns>
        public async Task<WebCallResult<ExanteSymbol>> GetSymbolAsync(string symbolId, CancellationToken ct = default)
        {
            symbolId.ValidateNotNull(nameof(symbolId));
            
            var url = GetUrl(symbolsEndpoint, dataEndpointType, apiVersion, symbolId);
            return await SendRequest<ExanteSymbol>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instrument schedule
        /// </summary>
        /// <returns>Financial schedule for requested instrument</returns>
        public async Task<WebCallResult<ExanteSchedule>> GetSymbolScheduleAsync(string symbolId, bool showAvailableOrderTypes = true, CancellationToken ct = default)
        {
            symbolId.ValidateNotNull(nameof(symbolId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"types", showAvailableOrderTypes}
                             };
            var url = GetUrl(string.Format(symbolsScheduleEndpoint, symbolId), dataEndpointType, apiVersion);
            return await SendRequest<ExanteSchedule>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instrument specification
        /// </summary>
        /// <returns>Additional parameters for instrument</returns>
        public async Task<WebCallResult<ExanteSpecification>> GetSymbolSpecificationAsync(string symbolId, CancellationToken ct = default)
        {
            symbolId.ValidateNotNull(nameof(symbolId));
            
            var url = GetUrl(string.Format(symbolsSpecificationEndpoint, symbolId), dataEndpointType, apiVersion);
            return await SendRequest<ExanteSpecification>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instrument types
        /// </summary>
        /// <returns>List of known instrument types</returns>
        public async Task<WebCallResult<IEnumerable<ExanteSymbolTypeId>>> GetSymbolAllTypesAsync(CancellationToken ct = default)
        {
            var url = GetUrl(typesEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteSymbolTypeId>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get instruments by type
        /// </summary>
        /// <returns>Financial instruments of the type</returns>
        public async Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsByTypeAsync(ExanteSymbolType type, CancellationToken ct = default)
        {
            var url = GetUrl(typesEndpoint, dataEndpointType, apiVersion, JsonConvert.SerializeObject(type, new SymbolTypeConverter(false)));
            return await SendRequest<IEnumerable<ExanteSymbol>>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        #endregion

        #region Live feed API

        #endregion

        #region Historical API

        #endregion

        #region Account summary API

        #endregion

        #region Transactions API

        #endregion

        #region Orders API

        #endregion

        #region Orders stream API

        #endregion

        #region Trades stream API

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