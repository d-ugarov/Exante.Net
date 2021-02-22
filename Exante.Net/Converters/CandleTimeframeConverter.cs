using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    public class CandleTimeframeConverter : JsonConverter
    {
        private readonly bool quotes;

        public CandleTimeframeConverter()
        {
            quotes = true;
        }

        public CandleTimeframeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteCandleTimeframe, string> values = new()
                                                                            {
                                                                                {ExanteCandleTimeframe.Minute1, "60"},
                                                                                {ExanteCandleTimeframe.Minute5, "300"},
                                                                                {ExanteCandleTimeframe.Minute10, "600"},
                                                                                {ExanteCandleTimeframe.Minute15, "900"},
                                                                                {ExanteCandleTimeframe.Minute30, "1800"},
                                                                                {ExanteCandleTimeframe.Hour1, "3600"},
                                                                                {ExanteCandleTimeframe.Hour4, "14400"},
                                                                                {ExanteCandleTimeframe.Hour6, "21600"},
                                                                                {ExanteCandleTimeframe.Day1, "86400"},
                                                                            };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteCandleTimeframe);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteCandleTimeframe)value!]);
            else
                writer.WriteRawValue(values[(ExanteCandleTimeframe)value!]);
        }
    }
}