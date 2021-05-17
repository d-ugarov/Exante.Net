using Exante.Net.Converters;
using Newtonsoft.Json;

namespace Exante.Net.Objects
{
    public class ExanteOption
    {
        /// <summary>
        /// Option group name
        /// </summary>
        public string OptionGroupId { get; set; } = "";
        
        /// <summary>
        /// Option strike price
        /// </summary>
        public decimal StrikePrice { get; set; }
        
        /// <summary>
        /// Option right
        /// </summary>
        [JsonConverter(typeof(OptionRightConverter))]
        public ExanteOptionRight OptionRight { get; set; }
    }
}