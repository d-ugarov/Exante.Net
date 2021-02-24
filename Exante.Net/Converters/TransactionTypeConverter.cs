using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class TransactionTypeConverter : JsonConverter
    {
        private readonly bool quotes;

        public TransactionTypeConverter()
        {
            quotes = true;
        }

        public TransactionTypeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteTransactionType, string> values = new()
                                                                            {
                                                                                {ExanteTransactionType.Trade, "TRADE"},
                                                                                {ExanteTransactionType.Commission, "COMMISSION"},
                                                                                {ExanteTransactionType.SubAccountTransfer, "SUBACCOUNT TRANSFER"},
                                                                                {ExanteTransactionType.Rollover, "ROLLOVER"},
                                                                                {ExanteTransactionType.Interest, "INTEREST"},
                                                                                {ExanteTransactionType.Dividend, "DIVIDEND"},
                                                                                {ExanteTransactionType.Tax, "TAX"},
                                                                                {ExanteTransactionType.Exercise, "EXERCISE"},
                                                                            };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteTransactionType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteTransactionType)value!]);
            else
                writer.WriteRawValue(values[(ExanteTransactionType)value!]);
        }
    }
}