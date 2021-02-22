using Newtonsoft.Json;

namespace Exante.Net.Objects
{
    public class ExantePositionCurrency
    {
        /// <summary>
        /// Currency
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Currency { get; set; } = "";
        
        /// <summary>
        /// Converted value of position if cross rates are available
        /// </summary>
        public decimal? ConvertedValue { get; set; }
        
        /// <summary>
        /// Value of position
        /// </summary>
        public decimal Value { get; set; }
    }
}