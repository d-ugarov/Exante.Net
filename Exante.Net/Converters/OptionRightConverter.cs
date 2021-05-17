using CryptoExchange.Net.Converters;
using Exante.Net.Objects;
using System.Collections.Generic;

namespace Exante.Net.Converters
{
    public class OptionRightConverter : BaseConverter<ExanteOptionRight>
    {
        public OptionRightConverter() : this(true)
        {
        }

        public OptionRightConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<ExanteOptionRight, string>> Mapping => new()
            {
                new KeyValuePair<ExanteOptionRight, string>(ExanteOptionRight.Call, "CALL"),
                new KeyValuePair<ExanteOptionRight, string>(ExanteOptionRight.Put, "PUT"),
            };
    }
}