using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteOrderTypes
    {
        [JsonProperty(ItemConverterType = typeof(OrderDurationConverter))]
        public IEnumerable<ExanteOrderDuration> Stop { get; set; } = new List<ExanteOrderDuration>();
        
        [JsonProperty(ItemConverterType = typeof(OrderDurationConverter))]
        public IEnumerable<ExanteOrderDuration> Limit { get; set; } = new List<ExanteOrderDuration>();
        
        [JsonProperty(ItemConverterType = typeof(OrderDurationConverter))]
        public IEnumerable<ExanteOrderDuration> Pegged { get; set; } = new List<ExanteOrderDuration>();
        
        [JsonProperty(ItemConverterType = typeof(OrderDurationConverter))]
        public IEnumerable<ExanteOrderDuration> Market { get; set; } = new List<ExanteOrderDuration>();
        
        [JsonProperty(PropertyName = "stop_limit", ItemConverterType = typeof(OrderDurationConverter))]
        public IEnumerable<ExanteOrderDuration> StopLimit { get; set; } = new List<ExanteOrderDuration>();
    }
}