using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteAccountSummary
    {
        /// <summary>
        /// Total NAV of user in the currency of the report
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// Open positions
        /// </summary>
        public IEnumerable<ExantePosition> Positions { get; set; } = new List<ExantePosition>();

        /// <summary>
        /// Currency of the report
        /// </summary>
        public string Currency { get; set; } = "";
        
        /// <summary>
        /// Margin utilization in fraction of NAV
        /// </summary>
        public decimal? MarginUtilization { get; set; }
        
        /// <summary>
        /// Date of the report
        /// </summary>
        [JsonProperty(PropertyName = "timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Money used for margin in the currency of the report
        /// </summary>
        public decimal? MoneyUsedForMargin { get; set; }

        /// <summary>
        /// Currencies on position
        /// </summary>
        public IEnumerable<ExantePositionCurrency> Currencies { get; set; } = new List<ExantePositionCurrency>();
        
        /// <summary>
        /// Session date of the report
        /// </summary>
        public string? SessionDate { get; set; }
        
        /// <summary>
        /// Free money in the currency of the report
        /// </summary>
        public decimal? FreeMoney { get; set; }

        /// <summary>
        /// User account id
        /// </summary>
        public string AccountId { get; set; } = "";
    }
}