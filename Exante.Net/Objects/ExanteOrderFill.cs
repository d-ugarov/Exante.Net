using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteOrderFill
    {
        /// <summary>
        /// Fill quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Fill serial number
        /// </summary>
        public int Position { get; set; }
        
        /// <summary>
        /// Fill date
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Fill price
        /// </summary>
        public decimal Price { get; set; }
    }
}