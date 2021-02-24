using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class ArrayOrderTypeConverter : JsonConverter
    {
        private readonly bool quotes;

        public ArrayOrderTypeConverter()
        {
            quotes = true;
        }

        public ArrayOrderTypeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteArrayOrderType, string> values = new()
                                                                           {
                                                                               {ExanteArrayOrderType.Asc, "ASC"},
                                                                               {ExanteArrayOrderType.Desc, "DESC"},
                                                                           };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteArrayOrderType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteArrayOrderType)value!]);
            else
                writer.WriteRawValue(values[(ExanteArrayOrderType)value!]);
        }
    }
}