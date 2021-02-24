using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteOrderParameters
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty(PropertyName = "orderType"), JsonConverter(typeof(OrderTypeConverter))]
        public ExanteOrderType Type { get; set; }
        
        /// <summary>
        /// Order limit price, limit orders only
        /// </summary>
        public decimal? LimitPrice { get; set; }
        
        /// <summary>
        /// Order side
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public ExanteOrderSide Side { get; set; }

        /// <summary>
        /// Associated instrument
        /// </summary>
        public string SymbolId { get; set; } = "";
        
        /// <summary>
        /// ID of an order on which this order depends
        /// </summary>
        [JsonProperty(PropertyName = "ifDoneParentId")]
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// Order place interval, twap orders only
        /// </summary>
        public int? PlaceInterval { get; set; }
        
        /// <summary>
        /// Order duration
        /// </summary>
        [JsonConverter(typeof(OrderDurationConverter))]
        public ExanteOrderDuration Duration { get; set; }
        
        /// <summary>
        /// Order stop price, stop orders only
        /// </summary>
        public decimal? StopPrice { get; set; }
        
        /// <summary>
        /// Order quantity
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Order expiration if applicable
        /// </summary>
        public DateTime? GttExpiration { get; set; }
        
        /// <summary>
        /// One-Cancels-the-Other group ID if set
        /// </summary>
        [JsonProperty(PropertyName = "ocoGroup")]
        public Guid? OcoGroupId { get; set; }
        
        /// <summary>
        /// Order price distance, trailing stop orders only
        /// </summary>
        public decimal? PriceDistance { get; set; }
        
        /// <summary>
        /// Order partial quantity, twap and iceberg orders only
        /// </summary>
        public decimal? PartQuantity { get; set; }
    }
}