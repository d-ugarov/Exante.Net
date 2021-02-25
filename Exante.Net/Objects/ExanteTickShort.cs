using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteTickShort
    {
        /// <summary>
        /// Date
        /// </summary>
        [JsonProperty(PropertyName = "timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Tick bid. Appears and required only for quote request
        /// </summary>
        public IEnumerable<ExanteQuote> Bid { get; set; } = new List<ExanteQuote>();

        /// <summary>
        /// Tick ask. Appears and required only for quote request
        /// </summary>
        public IEnumerable<ExanteQuote> Ask { get; set; } = new List<ExanteQuote>();

        /// <summary>
        /// Financial instrument id
        /// </summary>
        public string SymbolId { get; set; } = "";
    }
}