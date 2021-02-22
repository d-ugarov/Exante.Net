using CryptoExchange.Net.Objects;
using Exante.Net.Enums;
using Exante.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exante.Net.Interfaces
{
    public interface IExanteClient
    {
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
    }
}