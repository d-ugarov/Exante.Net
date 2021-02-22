using CryptoExchange.Net.Converters;
using Exante.Net.Enums;
using System.Collections.Generic;

namespace Exante.Net.Converters
{
    public class AccountStatusConverter : BaseConverter<ExanteAccountStatus>
    {
        public AccountStatusConverter() : this(true)
        {
        }

        public AccountStatusConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<ExanteAccountStatus, string>> Mapping => new()
            {
                new KeyValuePair<ExanteAccountStatus, string>(ExanteAccountStatus.ReadOnly, "ReadOnly"),
                new KeyValuePair<ExanteAccountStatus, string>(ExanteAccountStatus.CloseOnly, "CloseOnly"),
                new KeyValuePair<ExanteAccountStatus, string>(ExanteAccountStatus.Full, "Full"),
            };
    }
}