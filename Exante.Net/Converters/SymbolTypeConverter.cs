using CryptoExchange.Net.Converters;
using Exante.Net.Enums;
using System.Collections.Generic;

namespace Exante.Net.Converters
{
    internal class SymbolTypeConverter : BaseConverter<ExanteSymbolType>
    {
        public SymbolTypeConverter() : this(true)
        {
        }

        public SymbolTypeConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<ExanteSymbolType, string>> Mapping => new()
            {
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.FXSpot, "FX_SPOT"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Currency, "CURRENCY"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Index, "INDEX"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Stock, "STOCK"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Bond, "BOND"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Fund, "FUND"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Future, "FUTURE"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.Option, "OPTION"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.CFD, "CFD"),
                new KeyValuePair<ExanteSymbolType, string>(ExanteSymbolType.CalendarSpread, "CALENDAR_SPREAD"),
            };
    }
}