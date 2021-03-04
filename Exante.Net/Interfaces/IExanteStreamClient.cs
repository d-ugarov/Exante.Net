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
        /// <summary>
        /// Get quote stream
        /// </summary>
        /// <returns>Life quote stream for the specified financial instrument</returns>
        Task<WebCallResult<ExanteStreamSubscription>> GetQuoteStreamAsync(IEnumerable<string> symbolIds, 
            Action<ExanteTickShort> onNewQuote, ExanteQuoteLevel level = ExanteQuoteLevel.BestPrice, CancellationToken ct = default);
    }
}