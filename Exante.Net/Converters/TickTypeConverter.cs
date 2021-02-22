using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class TickTypeConverter : JsonConverter
    {
        private readonly bool quotes;

        public TickTypeConverter()
        {
            quotes = true;
        }

        public TickTypeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteTickType, string> values = new()
                                                                     {
                                                                         {ExanteTickType.Quotes, "quotes"},
                                                                         {ExanteTickType.Trades, "trades"},
                                                                     };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteTickType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteTickType)value!]);
            else
                writer.WriteRawValue(values[(ExanteTickType)value!]);
        }
    }
}