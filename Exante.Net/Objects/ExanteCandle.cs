using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteCandle
    {
        /// <summary>
        /// Candle close price
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Candle open price
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// Candle low price
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Candle high price
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Total volume for specified period. Appears and required only for trade candle request
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Candle date
        /// </summary>
        [JsonProperty(PropertyName = "timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }
    }
}