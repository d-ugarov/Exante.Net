using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;

namespace Exante.Net.Objects
{
    public class ExantePosition
    {
        /// <summary>
        /// Current position PnL in the currency
        /// </summary>
        public decimal? ConvertedPnL { get; set; }
        
        /// <summary>
        /// Financial instrument type
        /// </summary>
        [JsonConverter(typeof(SymbolTypeConverter))]
        public ExanteSymbolType SymbolType { get; set; }

        /// <summary>
        /// Currency code of the financial instrument
        /// </summary>
        public string Currency { get; set; } = "";
        
        /// <summary>
        /// Current position PnL
        /// </summary>
        public decimal? PnL { get; set; }
        
        /// <summary>
        /// Current financial instrument price
        /// </summary>
        public decimal? Price { get; set; }
        
        /// <summary>
        /// Quantity on position
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Financial instrument identifier
        /// </summary>
        public string SymbolId { get; set; } = "";
        
        /// <summary>
        /// Position value in the currency
        /// </summary>
        public decimal? ConvertedValue { get; set; }
        
        /// <summary>
        /// Average position opening price
        /// </summary>
        public decimal? AveragePrice { get; set; }
        
        /// <summary>
        /// Position value
        /// </summary>
        public decimal? Value { get; set; }
    }
}