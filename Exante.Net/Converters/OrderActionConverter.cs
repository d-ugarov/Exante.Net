using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class OrderActionConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderActionConverter()
        {
            quotes = true;
        }

        public OrderActionConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteOrderAction, string> values = new()
                                                                        {
                                                                            {ExanteOrderAction.Cancel, "cancel"},
                                                                            {ExanteOrderAction.Replace, "replace"},
                                                                        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteOrderAction);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteOrderAction)value!]);
            else
                writer.WriteRawValue(values[(ExanteOrderAction)value!]);
        }
    }
}