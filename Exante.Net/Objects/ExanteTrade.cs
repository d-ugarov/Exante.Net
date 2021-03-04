using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteTrade
    {
        /// <summary>
        /// Trade quantity
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Trade price
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Order fill serial number for the trade
        /// </summary>
        public int Position { get; set; }
        
        /// <summary>
        /// Respected order ID
        /// </summary>
        public Guid OrderId { get; set; }
        
        /// <summary>
        /// Date
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Date { get; set; }

        internal bool IsEmpty => OrderId == default &&
                                 Date == default;
    }
}