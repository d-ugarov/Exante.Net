using CryptoExchange.Net.Objects;
using Exante.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exante.Net.Interfaces
{
    public interface IExanteClient
    {
        /// <summary>
        /// Get exchanges
        /// </summary>
        Task<WebCallResult<IEnumerable<ExanteExchange>>> GetExchangesAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Get instruments by exchange
        /// </summary>
        Task<WebCallResult<IEnumerable<ExanteSymbolInfo>>> GetExchangeInstrumentsAsync(string exchangeId, CancellationToken ct = default);
    }
}