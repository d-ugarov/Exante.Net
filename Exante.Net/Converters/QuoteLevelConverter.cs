using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class QuoteLevelConverter : JsonConverter
    {
        private readonly bool quotes;

        public QuoteLevelConverter()
        {
            quotes = true;
        }

        public QuoteLevelConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteQuoteLevel, string> values = new()
                                                                       {
                                                                           {ExanteQuoteLevel.BestPrice, "best_price"},
                                                                           {ExanteQuoteLevel.MarketDepth, "market_depth"},
                                                                       };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteQuoteLevel);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteQuoteLevel)value!]);
            else
                writer.WriteRawValue(values[(ExanteQuoteLevel)value!]);
        }
    }
}