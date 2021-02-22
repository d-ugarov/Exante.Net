using CryptoExchange.Net.Converters;
using Exante.Net.Enums;
using System.Collections.Generic;

namespace Exante.Net.Converters
{
    public class OrderDurationConverter : BaseConverter<ExanteOrderDuration>
    {
        public OrderDurationConverter() : this(true)
        {
        }

        public OrderDurationConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<ExanteOrderDuration, string>> Mapping => new()
            {
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.Day, "day"),
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.AtTheClose, "at_the_close"),
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.AtTheOpening, "at_the_opening"),
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.FillOrKill, "fill_or_kill"),
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.ImmediateOrCancel, "immediate_or_cancel"),
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.GoodTillCancel, "good_till_cancel"),
                new KeyValuePair<ExanteOrderDuration, string>(ExanteOrderDuration.GoodTillTime, "good_till_time"),
            };
    }
}