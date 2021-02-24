using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class OrderDurationConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderDurationConverter()
        {
            quotes = true;
        }

        public OrderDurationConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteOrderDuration, string> values = new()
                                                                          {
                                                                              {ExanteOrderDuration.Day, "day"},
                                                                              {ExanteOrderDuration.AtTheClose, "at_the_close"},
                                                                              {ExanteOrderDuration.AtTheOpening, "at_the_opening"},
                                                                              {ExanteOrderDuration.FillOrKill, "fill_or_kill"},
                                                                              {ExanteOrderDuration.ImmediateOrCancel, "immediate_or_cancel"},
                                                                              {ExanteOrderDuration.GoodTillCancel, "good_till_cancel"},
                                                                              {ExanteOrderDuration.GoodTillTime, "good_till_time"},
                                                                          };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteOrderDuration);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteOrderDuration)value!]);
            else
                writer.WriteRawValue(values[(ExanteOrderDuration)value!]);
        }
    }
}