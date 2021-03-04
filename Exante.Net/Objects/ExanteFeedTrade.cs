using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteFeedTrade
    {
        /// <summary>
        /// Trade size
        /// </summary>
        public decimal? Size { get; set; }
        
        /// <summary>
        /// Financial instrument id
        /// </summary>
        public string SymbolId { get; set; } = "";
        
        /// <summary>
        /// Trade price
        /// </summary>
        public decimal? Price { get; set; }
        
        /// <summary>
        /// Date
        /// </summary>
        [JsonProperty(PropertyName = "timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }

        internal bool IsEmpty => !Size.HasValue &&
                                 string.IsNullOrEmpty(SymbolId) &&
                                 !Price.HasValue &&
                                 Date == default;
    }
}