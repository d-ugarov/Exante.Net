using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteOrder
    {
        /// <summary>
        /// Current order modification unique ID
        /// </summary>
        public Guid CurrentModificationId { get; set; }

        /// <summary>
        /// Order place time
        /// </summary>
        [JsonProperty(PropertyName = "placeTime")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Associated name
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Unique order ID
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Order state
        /// </summary>
        public ExanteOrderState OrderState { get; set; } = null!;

        /// <summary>
        /// Order response parameters
        /// </summary>
        public ExanteOrderParameters OrderParameters { get; set; } = null!;

        /// <summary>
        /// Optional client tag to identify or group orders
        /// </summary>
        public string? ClientTag { get; set; }

        /// <summary>
        /// Associated account ID
        /// </summary>
        public string AccountId { get; set; } = "";
    }
}