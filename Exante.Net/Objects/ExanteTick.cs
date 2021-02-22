using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteTick
    {
        /// <summary>
        /// Trade price. Appears and required only for trade request
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [JsonProperty(PropertyName = "timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Tick bid. Appears and required only for quote request
        /// </summary>
        public IEnumerable<ExanteQuote>? Bid { get; set; }

        /// <summary>
        /// Tick ask. Appears and required only for quote request
        /// </summary>
        public IEnumerable<ExanteQuote>? Ask { get; set; }

        /// <summary>
        /// Financial instrument id
        /// </summary>
        public string SymbolId { get; set; } = "";

        /// <summary>
        /// Trade size. Appears and required only for trade request
        /// </summary>
        public decimal? Size { get; set; }
    }
}