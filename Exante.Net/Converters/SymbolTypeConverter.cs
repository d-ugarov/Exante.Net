using Exante.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exante.Net.Converters
{
    internal class SymbolTypeConverter : JsonConverter
    {
        private readonly bool quotes;

        public SymbolTypeConverter()
        {
            quotes = true;
        }

        public SymbolTypeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ExanteSymbolType, string> values = new()
                                                                       {
                                                                           {ExanteSymbolType.FXSpot, "FX_SPOT"},
                                                                           {ExanteSymbolType.Currency, "CURRENCY"},
                                                                           {ExanteSymbolType.Index, "INDEX"},
                                                                           {ExanteSymbolType.Stock, "STOCK"},
                                                                           {ExanteSymbolType.Bond, "BOND"},
                                                                           {ExanteSymbolType.Fund, "FUND"},
                                                                           {ExanteSymbolType.Future, "FUTURE"},
                                                                           {ExanteSymbolType.Option, "OPTION"},
                                                                           {ExanteSymbolType.CFD, "CFD"},
                                                                           {ExanteSymbolType.CalendarSpread, "CALENDAR_SPREAD"}
                                                                       };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ExanteSymbolType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return values.Single(v => v.Value == (string?)reader.Value).Key;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ExanteSymbolType)value!]);
            else
                writer.WriteRawValue(values[(ExanteSymbolType)value!]);
        }
    }
}