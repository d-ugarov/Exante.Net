using CryptoExchange.Net.Objects;
using Exante.Net.Enums;
using Exante.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exante.Net.Interfaces
{
    public interface IExanteClient
    {
        #region Accounts API

        /// <summary>
        /// Get user accounts
        /// </summary>
        /// <returns>List of user accounts and their statuses</returns>
        Task<WebCallResult<IEnumerable<ExanteAccount>>> GetAccountsAsync(CancellationToken ct = default);

        #endregion

        #region Daily change API
        
        /// <summary>
        /// Get daily changes
        /// <remarks>Returns all daily changes if symbols is empty</remarks>
        /// </summary>
        /// <returns>List of daily changes</returns>
        Task<WebCallResult<IEnumerable<ExanteDailyChange>>> GetDailyChangesAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);

        #endregion

        #region Crossrates API
        
        /// <summary>
        /// Get list of available currencies
        /// </summary>
        /// <returns>List of available currencies</returns>
        Task<WebCallResult<ExanteAvailableCrossRates>> GetAvailableCurrenciesAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Get cross rate
        /// </summary>
        /// <returns>Cross rate from one currency to another</returns>
        Task<WebCallResult<ExanteCrossRate>> GetCrossRateAsync(string from, string to, CancellationToken ct = default);

        #endregion
        
        #region Symbols API
        
        /// <summary>
        /// Get exchanges
        /// </summary>
        /// <returns>List of exchanges</returns>
        Task<WebCallResult<IEnumerable<ExanteExchange>>> GetExchangesAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Get instruments by exchange
        /// </summary>
        /// <returns>Exchange financial instruments</returns>
        Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsByExchangeAsync(string exchangeId, CancellationToken ct = default);
        
        /// <summary>
        /// Get instrument groups
        /// </summary>
        /// <returns>Available instrument groups</returns>
        Task<WebCallResult<IEnumerable<ExanteInstrumentGroup>>> GetSymbolAllGroupsAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Get instruments by group
        /// </summary>
        /// <returns>Financial instruments which belong to specified group</returns>
        Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsByGroupAsync(string groupId, CancellationToken ct = default);
        
        /// <summary>
        /// Get nearest expiration in group
        /// </summary>
        /// <returns>Financial instrument which has the nearest expiration in the group</returns>
        Task<WebCallResult<ExanteSymbol>> GetSymbolNearestExpirationInGroupAsync(string groupId, CancellationToken ct = default);

        /// <summary>
        /// Get instrument list
        /// </summary>
        /// <returns>List of instruments available for authorized user</returns>
        Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get instrument
        /// </summary>
        /// <returns>Instrument available for authorized user</returns>
        Task<WebCallResult<ExanteSymbol>> GetSymbolAsync(string symbolId, CancellationToken ct = default);
        
        /// <summary>
        /// Get instrument schedule
        /// </summary>
        /// <returns>Financial schedule for requested instrument</returns>
        Task<WebCallResult<ExanteSchedule>> GetSymbolScheduleAsync(string symbolId, bool showAvailableOrderTypes = true, CancellationToken ct = default);
        
        /// <summary>
        /// Get instrument specification
        /// </summary>
        /// <returns>Additional parameters for instrument</returns>
        Task<WebCallResult<ExanteSpecification>> GetSymbolSpecificationAsync(string symbolId, CancellationToken ct = default);

        /// <summary>
        /// Get instrument types
        /// </summary>
        /// <returns>List of known instrument types</returns>
        Task<WebCallResult<IEnumerable<ExanteSymbolTypeId>>> GetSymbolAllTypesAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Get instruments by type
        /// </summary>
        /// <returns>Financial instruments of the type</returns>
        Task<WebCallResult<IEnumerable<ExanteSymbol>>> GetSymbolsByTypeAsync(ExanteSymbolType type, CancellationToken ct = default);

        #endregion

        #region Live feed API

        /// <summary>
        /// Get last quote
        /// </summary>
        /// <returns>Last quote for the specified financial instrument</returns>
        Task<WebCallResult<IEnumerable<ExanteTickShort>>> GetLastQuoteAsync(IEnumerable<string> symbolIds,
            ExanteQuoteLevel level = ExanteQuoteLevel.BestPrice, CancellationToken ct = default);

        #endregion

        #region Historical API

        /// <summary>
        /// Get OHLC candles
        /// </summary>
        /// <returns>List of OHLC candles for the specified financial instrument and duration</returns>
        Task<WebCallResult<IEnumerable<ExanteCandle>>> GetCandlesAsync(string symbolId, ExanteCandleTimeframe timeframe,
            DateTime? from = null, DateTime? to = null, int limit = 60, ExanteTickType tickType = ExanteTickType.Quotes, CancellationToken ct = default);
        
        /// <summary>
        /// Get ticks
        /// </summary>
        /// <returns>List of ticks for the specified financial instrument</returns>
        Task<WebCallResult<IEnumerable<ExanteTick>>> GetTicksAsync(string symbolId, DateTime? from = null, 
            DateTime? to = null, int limit = 60, ExanteTickType tickType = ExanteTickType.Quotes, CancellationToken ct = default);

        #endregion

        #region Account summary API

        /// <summary>
        /// Get account summary
        /// </summary>
        /// <returns>Summary for the specified account</returns>
        Task<WebCallResult<ExanteAccountSummary>> GetAccountSummaryAsync(string accountId, string currency,
            DateTime? date = null, CancellationToken ct = default);

        #endregion

        #region Transactions API
        
        /// <summary>
        /// Get transactions
        /// </summary>
        /// <returns>List of transactions with the specified filter</returns>
        Task<WebCallResult<IEnumerable<ExanteTransaction>>> GetTransactionsAsync(Guid? transactionId = null,
            string? accountId = null, string? symbolId = null, string? asset = null, IEnumerable<ExanteTransactionType>? types = null,
            int? offset = null, int? limit = null, ExanteArrayOrderType orderType = ExanteArrayOrderType.Desc, 
            DateTime? from = null, DateTime? to = null, Guid? orderId = null, int? orderPosition = null, CancellationToken ct = default);

        #endregion

        #region Orders API

        /// <summary>
        /// Get historical orders
        /// </summary>
        /// <returns>List of historical orders</returns>
        Task<WebCallResult<IEnumerable<ExanteOrder>>> GetOrdersAsync(string? accountId = null,
            int? limit = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);

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
        Task<WebCallResult<IEnumerable<ExanteOrder>>> PlaceOrderAsync(string accountId, string symbolId, 
            ExanteOrderType type, ExanteOrderSide side, decimal quantity, ExanteOrderDuration duration, 
            decimal? limitPrice = null, decimal? stopPrice = null, decimal? stopLoss = null, decimal? takeProfit = null,  
            int? placeInterval = null, string? clientTag = null, Guid? parentId = null, Guid? ocoGroupId = null, 
            DateTime? gttExpiration = null, int? priceDistance = null, decimal? partQuantity = null, CancellationToken ct = default);

        /// <summary>
        /// Get active orders
        /// </summary>
        /// <returns>List of active trading orders</returns>
        Task<WebCallResult<IEnumerable<ExanteOrder>>> GetActiveOrdersAsync(string? accountId = null,
            string? symbolId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get order
        /// </summary>
        /// <returns>Order with specified identifier</returns>
        Task<WebCallResult<ExanteOrder>> GetOrderAsync(Guid orderId, CancellationToken ct = default);

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
        Task<WebCallResult<ExanteOrder>> ModifyOrderAsync(Guid orderId, ExanteOrderAction action,
            decimal? quantity = null, decimal? stopPrice = null, int? priceDistance = null, decimal? limitPrice = null,
            CancellationToken ct = default);

        #endregion

        #region Orders stream API

        #endregion

        #region Trades stream API

        #endregion
    }
}