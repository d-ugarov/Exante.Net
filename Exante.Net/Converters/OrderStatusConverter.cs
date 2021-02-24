using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class OrderStatusConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderStatusConverter()
        {
            quotes = true;
        }

        public OrderStatusConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteOrderStatus, string> values = new()
                                                                        {
                                                                            {ExanteOrderStatus.Placing, "placing"},
                                                                            {ExanteOrderStatus.Working, "working"},
                                                                            {ExanteOrderStatus.Cancelled, "cancelled"},
                                                                            {ExanteOrderStatus.Pending, "pending"},
                                                                            {ExanteOrderStatus.Filled, "filled"},
                                                                            {ExanteOrderStatus.Rejected, "rejected"},
                                                                        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteOrderStatus);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteOrderStatus)value!]);
            else
                writer.WriteRawValue(values[(ExanteOrderStatus)value!]);
        }
    }
}