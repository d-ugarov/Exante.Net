using CryptoExchange.Net.Converters;
using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System;

namespace Exante.Net.Objects
{
    public class ExanteTransaction
    {
        /// <summary>
        /// Transaction financial instrument
        /// </summary>
        public string SymbolId { get; set; } = "";

        /// <summary>
        /// Transaction type
        /// </summary>
        [JsonProperty(PropertyName = "operationType"), JsonConverter(typeof(TransactionTypeConverter))]
        public ExanteTransactionType Type { get; set; }

        [JsonProperty(PropertyName = "timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Transaction asset
        /// </summary>
        public string Asset { get; set; } = "";

        /// <summary>
        /// Transaction id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Transaction account id
        /// </summary>
        public string AccountId { get; set; } = "";

        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty(PropertyName = "sum")]
        public decimal Amount { get; set; }
    }
}