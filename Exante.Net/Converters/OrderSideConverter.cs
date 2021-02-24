using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class OrderSideConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderSideConverter()
        {
            quotes = true;
        }

        public OrderSideConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteOrderSide, string> values = new()
                                                                      {
                                                                          {ExanteOrderSide.Buy, "buy"},
                                                                          {ExanteOrderSide.Sell, "sell"},
                                                                      };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteOrderSide);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteOrderSide)value!]);
            else
                writer.WriteRawValue(values[(ExanteOrderSide)value!]);
        }
    }
}