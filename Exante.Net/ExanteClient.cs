using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Exante.Net.Converters;
using Exante.Net.Enums;
using Exante.Net.Interfaces;
using Exante.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private const string feedLastEndpoint = "feed/{0}/last";
        private const string ohlcEndpoint = "ohlc/{0}/{1}";
        private const string ticksEndpoint = "ticks";
        private const string typesEndpoint = "types";
        private const string accountSummaryEndpoint = "summary/{0}/{1}";
        private const string accountByDateSummaryEndpoint = "summary/{0}/{1}/{2}";
        private const string transactionsEndpoint = "transactions";
        private const string ordersEndpoint = "orders";
        private const string ordersActiveEndpoint = "orders/active";

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
            requestBodyFormat = RequestBodyFormat.Json;
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
        
        /// <summary>
        /// Get live feed last quote
        /// </summary>
        /// <returns>Last quote for the specified financial instrument</returns>
        public async Task<WebCallResult<IEnumerable<ExanteTickShort>>> GetFeedLastQuoteAsync(IEnumerable<string> symbolIds, 
            ExanteQuoteLevel level = ExanteQuoteLevel.BestPrice, CancellationToken ct = default)
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
            
            var url = GetUrl(string.Format(feedLastEndpoint, string.Join(",", symbols.Distinct())), dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteTickShort>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Historical API

        /// <summary>
        /// Get OHLC candles
        /// </summary>
        /// <returns>List of OHLC candles for the specified financial instrument and duration</returns>
        public async Task<WebCallResult<IEnumerable<ExanteCandle>>> GetCandlesAsync(string symbolId, ExanteCandleTimeframe timeframe,
            DateTime? from = null, DateTime? to = null, int limit = 60, ExanteTickType tickType = ExanteTickType.Quotes, CancellationToken ct = default)
        {
            symbolId.ValidateNotNull(nameof(symbolId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"size", limit.ToString(CultureInfo.InvariantCulture)},
                                 {"type", JsonConvert.SerializeObject(tickType, new TickTypeConverter(false))},
                             };
            parameters.AddOptionalParameter("from", from.HasValue ? JsonConvert.SerializeObject(from, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("to", to.HasValue ? JsonConvert.SerializeObject(to, new TimestampConverter()) : null);
            
            var endpoint = string.Format(ohlcEndpoint, symbolId, JsonConvert.SerializeObject(timeframe, new CandleTimeframeConverter(false)));
            var url = GetUrl(endpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteCandle>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get ticks
        /// </summary>
        /// <returns>List of ticks for the specified financial instrument</returns>
        public async Task<WebCallResult<IEnumerable<ExanteTick>>> GetTicksAsync(string symbolId, DateTime? from = null, 
            DateTime? to = null, int limit = 60, ExanteTickType tickType = ExanteTickType.Quotes, CancellationToken ct = default)
        {
            symbolId.ValidateNotNull(nameof(symbolId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"size", limit.ToString(CultureInfo.InvariantCulture)},
                                 {"type", JsonConvert.SerializeObject(tickType, new TickTypeConverter(false))},
                             };
            parameters.AddOptionalParameter("from", from.HasValue ? JsonConvert.SerializeObject(from, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("to", to.HasValue ? JsonConvert.SerializeObject(to, new TimestampConverter()) : null);
            
            var url = GetUrl(ticksEndpoint, dataEndpointType, apiVersion, symbolId);
            return await SendRequest<IEnumerable<ExanteTick>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Account summary API

        /// <summary>
        /// Get account summary
        /// </summary>
        /// <returns>Summary for the specified account</returns>
        public async Task<WebCallResult<ExanteAccountSummary>> GetAccountSummaryAsync(string accountId, string currency,
            DateTime? date = null, CancellationToken ct = default)
        {
            accountId.ValidateNotNull(nameof(accountId));
            currency.ValidateNotNull(nameof(currency));

            var endpoint = date.HasValue
                ? string.Format(accountByDateSummaryEndpoint, accountId, date.Value.ToString("yyyy-MM-dd"), currency.ToUpperInvariant())
                : string.Format(accountSummaryEndpoint, accountId, currency.ToUpperInvariant());

            var url = GetUrl(endpoint, dataEndpointType, apiVersion);
            return await SendRequest<ExanteAccountSummary>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        #endregion

        #region Transactions API
        
        /// <summary>
        /// Get transactions
        /// </summary>
        /// <returns>List of transactions with the specified filter</returns>
        public async Task<WebCallResult<IEnumerable<ExanteTransaction>>> GetTransactionsAsync(Guid? transactionId = null,
            string? accountId = null, string? symbolId = null, string? asset = null, IEnumerable<ExanteTransactionType>? types = null,
            int? offset = null, int? limit = null, ExanteArrayOrderType orderType = ExanteArrayOrderType.Desc, 
            DateTime? from = null, DateTime? to = null, Guid? orderId = null, int? orderPosition = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();

            parameters.AddOptionalParameter("uuid", transactionId);
            parameters.AddOptionalParameter("accountId", accountId);
            parameters.AddOptionalParameter("symbolId", symbolId);
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("operationType", types != null
                ? string.Join(",", types.Select(x => JsonConvert.SerializeObject(x, new TransactionTypeConverter(false))))
                : null);
            parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("order", JsonConvert.SerializeObject(orderType, new ArrayOrderTypeConverter(false)));
            parameters.AddOptionalParameter("fromDate", from?.ToString("yyyy-MM-dd"));
            parameters.AddOptionalParameter("toDate", to?.ToString("yyyy-MM-dd"));
            parameters.AddOptionalParameter("orderId", orderId);
            parameters.AddOptionalParameter("orderPos", orderPosition?.ToString(CultureInfo.InvariantCulture));

            var url = GetUrl(transactionsEndpoint, dataEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteTransaction>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Orders API
        
        /// <summary>
        /// Get historical orders
        /// </summary>
        /// <returns>List of historical orders</returns>
        public async Task<WebCallResult<IEnumerable<ExanteOrder>>> GetOrdersAsync(string? accountId = null, 
            int? limit = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();

            parameters.AddOptionalParameter("accountId", accountId);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("from", from?.ToString("yyyy-MM-dd"));
            parameters.AddOptionalParameter("to", to?.ToString("yyyy-MM-dd"));

            var url = GetUrl(ordersEndpoint, tradeEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteOrder>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Place order
        /// </summary>
        /// <param name="accountId">User account to place order</param>
        /// <param name="symbolId">Order instrument</param>
        /// <param name="type">Order type</param>
        /// <param name="side">Order side</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="duration">Order duration</param>
        /// <param name="limitPrice">Order limit price if applicable</param>
        /// <param name="stopPrice">Order stop price if applicable</param>
        /// <param name="stopLoss">Optional price of stop loss order</param>
        /// <param name="takeProfit">Optional price of take profit order</param>
        /// <param name="placeInterval">Order place interval, twap orders only</param>
        /// <param name="clientTag">Optional client tag to identify or group orders</param>
        /// <param name="parentId">ID of an order on which this order depends</param>
        /// <param name="ocoGroupId">One-Cancels-the-Other group ID if set</param>
        /// <param name="gttExpiration">Order expiration if applicable</param>
        /// <param name="priceDistance">Order price distance, trailing stop orders only</param>
        /// <param name="partQuantity">Order partial quantity, twap and Iceberg orders only</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>New trading order</returns>
        public async Task<WebCallResult<IEnumerable<ExanteOrder>>> PlaceOrderAsync(string accountId, string symbolId, 
            ExanteOrderType type, ExanteOrderSide side, decimal quantity, ExanteOrderDuration duration, 
            decimal? limitPrice = null, decimal? stopPrice = null, decimal? stopLoss = null, decimal? takeProfit = null,  
            int? placeInterval = null, string? clientTag = null, Guid? parentId = null, Guid? ocoGroupId = null, 
            DateTime? gttExpiration = null, int? priceDistance = null, decimal? partQuantity = null, CancellationToken ct = default)
        {
            accountId.ValidateNotNull(nameof(accountId));
            symbolId.ValidateNotNull(nameof(symbolId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"accountId", accountId},
                                 {"symbolId", symbolId},
                                 {"orderType", JsonConvert.SerializeObject(type, new OrderTypeConverter(false))},
                                 {"side", JsonConvert.SerializeObject(side, new OrderSideConverter(false))},
                                 {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                                 {"duration", JsonConvert.SerializeObject(duration, new OrderDurationConverter(false))},
                             };
            parameters.AddOptionalParameter("limitPrice", limitPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("stopLoss", stopLoss?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("takeProfit", takeProfit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("placeInterval", placeInterval?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientTag", clientTag);
            parameters.AddOptionalParameter("ifDoneParentId", parentId);
            parameters.AddOptionalParameter("ocoGroup", ocoGroupId);
            parameters.AddOptionalParameter("gttExpiration", gttExpiration?.ToString("O"));
            parameters.AddOptionalParameter("priceDistance", priceDistance?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("partQuantity", partQuantity?.ToString(CultureInfo.InvariantCulture));
            
            var url = GetUrl(ordersEndpoint, tradeEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteOrder>>(url, HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get active orders
        /// </summary>
        /// <returns>List of active trading orders</returns>
        public async Task<WebCallResult<IEnumerable<ExanteOrder>>> GetActiveOrdersAsync(string? accountId = null, 
            string? symbolId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();

            parameters.AddOptionalParameter("accountId", accountId);
            parameters.AddOptionalParameter("symbolId", symbolId);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var url = GetUrl(ordersActiveEndpoint, tradeEndpointType, apiVersion);
            return await SendRequest<IEnumerable<ExanteOrder>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get order
        /// </summary>
        /// <returns>Order with specified identifier</returns>
        public async Task<WebCallResult<ExanteOrder>> GetOrderAsync(Guid orderId, CancellationToken ct = default)
        {
            var url = GetUrl(ordersEndpoint, tradeEndpointType, apiVersion, orderId.ToString());
            return await SendRequest<ExanteOrder>(url, HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Modify order
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="action">Order modification action</param>
        /// <param name="quantity">New order quantity to replace</param>
        /// <param name="stopPrice">New order stop price if applicable</param>
        /// <param name="priceDistance">New order price distance if applicable</param>
        /// <param name="limitPrice">New order limit price if applicable</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ExanteOrder>> ModifyOrderAsync(Guid orderId, ExanteOrderAction action,
            decimal? quantity = null, decimal? stopPrice = null, int? priceDistance = null, decimal? limitPrice = null,
            CancellationToken ct = default)
        {
            if (action == ExanteOrderAction.Replace && !quantity.HasValue)
                throw new ArgumentException("Quantity must be sent");

            var replaceParameters = new
                                    {
                                        quantity = quantity?.ToString(CultureInfo.InvariantCulture),
                                        stopPrice = stopPrice?.ToString(CultureInfo.InvariantCulture),
                                        priceDistance = priceDistance?.ToString(CultureInfo.InvariantCulture),
                                        limitPrice = limitPrice?.ToString(CultureInfo.InvariantCulture),
                                    };
            var parameters = new Dictionary<string, object>
                             {
                                 {"action", JsonConvert.SerializeObject(action, new OrderActionConverter(false))},
                             };
            parameters.AddOptionalParameter("parameters", action == ExanteOrderAction.Cancel ? null : replaceParameters);

            var url = GetUrl(ordersEndpoint, tradeEndpointType, apiVersion, orderId.ToString());
            return await SendRequest<ExanteOrder>(url, HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
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