using CryptoExchange.Net.Converters;
using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteSymbol
    {
        /// <summary>
        /// Short symbol description
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Exchange id where the symbol is traded
        /// </summary>
        public string? Exchange { get; set; }
        
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonConverter(typeof(SymbolTypeConverter))]
        public ExanteSymbolType SymbolType { get; set; }
        
        /// <summary>
        /// Internal symbol id
        /// </summary>
        public string SymbolId { get; set; } = "";
        
        /// <summary>
        /// Expiration date if applicable
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime? Expiration { get; set; }
        
        /// <summary>
        /// Country of symbol's exchange
        /// </summary>
        public string? Country { get; set; }
        
        /// <summary>
        /// Group of symbol, applicable to futures and options
        /// </summary>
        public string? Group { get; set; }
        
        /// <summary>
        /// Long symbol description
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Currency of symbol price
        /// </summary>
        public string Currency { get; set; } = "";

        /// <summary>
        /// Minimum possible increment of symbol price
        /// </summary>
        public decimal MinPriceIncrement { get; set; }
        
        /// <summary>
        /// Exchange ticker
        /// </summary>
        public string Ticker { get; set; } = "";
        
        /// <summary>
        /// Option specific properties
        /// </summary>
        public ExanteOption? OptionData { get; set; }
    }
}