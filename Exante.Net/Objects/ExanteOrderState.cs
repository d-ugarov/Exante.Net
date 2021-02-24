using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteOrderState
    {
        /// <summary>
        /// Order last update time
        /// </summary>
        public DateTime LastUpdate { get; set; }
        
        /// <summary>
        /// Current order status
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public ExanteOrderStatus Status { get; set; }
        
        /// <summary>
        /// Order rejected reason if applicable
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Array of order fills
        /// </summary>
        public IEnumerable<ExanteOrderFill> Fills { get; set; } = new List<ExanteOrderFill>();
    }
}