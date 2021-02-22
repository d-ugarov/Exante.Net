using Newtonsoft.Json;

namespace Exante.Net.Objects
{
    public class ExanteCrossRate
    {
        /// <summary>
        /// Cross rate pair
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public string Symbol { get; set; } = "";
        
        /// <summary>
        /// Symbol id, which can be used to request history or subscribe to feed
        /// </summary>
        public string? SymbolId { get; set; }
        
        /// <summary>
        /// Current cross rate
        /// </summary>
        public decimal Rate { get; set; }
    }
}