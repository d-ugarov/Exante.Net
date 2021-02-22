using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExantePeriod
    {
        /// <summary>
        /// Session start
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Start { get; set; }
        
        /// <summary>
        /// Session end
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime End { get; set; }
    }
}