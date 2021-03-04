using CryptoExchange.Net.Objects;
using Exante.Net.Enums;
using Exante.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exante.Net.Interfaces
{
    public interface IExanteStreamClient
    {
        #region Live feed API

        /// <summary>
        /// Get live feed trades stream
        /// </summary>
        /// <returns>Trades stream for the specified financial instrument</returns>
        Task<WebCallResult<ExanteStreamSubscription>> GetFeedTradesStreamAsync(IEnumerable<string> symbolIds,
            Action<ExanteFeedTrade> onNewTrade, CancellationToken ct = default);
        
        /// <summary>
        /// Get live feed quote stream
        /// </summary>
        /// <returns>Life quote stream for the specified financial instrument</returns>
        Task<WebCallResult<ExanteStreamSubscription>> GetFeedQuoteStreamAsync(IEnumerable<string> symbolIds, 
            Action<ExanteTickShort> onNewQuote, ExanteQuoteLevel level = ExanteQuoteLevel.BestPrice, CancellationToken ct = default);
        
        #endregion

        #region Orders stream API

        /// <summary>
        /// Get order updates stream
        /// </summary>
        Task<WebCallResult<ExanteStreamSubscription>> GetOrdersStreamAsync(Action<ExanteOrder> onNewOrder, CancellationToken ct = default);

        #endregion

        #region Trades stream API

        /// <summary>
        /// Get trades stream
        /// </summary>
        Task<WebCallResult<ExanteStreamSubscription>> GetTradesStreamAsync(Action<ExanteTrade> onNewTrade, CancellationToken ct = default);
        
        #endregion

        #region Stream common

        CallResult StopStream(ExanteStreamSubscription subscription);

        #endregion

        #region Common

        void SetApiCredentials(string clientId, string applicationId, string sharedKey);

        #endregion
    }
}