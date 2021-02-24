using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class OrderTypeConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderTypeConverter()
        {
            quotes = true;
        }

        public OrderTypeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteOrderType, string> values = new()
                                                                      {
                                                                          {ExanteOrderType.Market, "market"},
                                                                          {ExanteOrderType.Limit, "limit"},
                                                                          {ExanteOrderType.Stop, "stop"},
                                                                          {ExanteOrderType.StopLimit, "stop_limit"},
                                                                          {ExanteOrderType.Twap, "twap"},
                                                                          {ExanteOrderType.TrailingStop, "trailing_stop"},
                                                                          {ExanteOrderType.Iceberg, "iceberg"},
                                                                      };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteOrderType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteOrderType)value!]);
            else
                writer.WriteRawValue(values[(ExanteOrderType)value!]);
        }
    }
}