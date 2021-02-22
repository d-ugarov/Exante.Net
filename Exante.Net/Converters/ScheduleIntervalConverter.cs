using CryptoExchange.Net.Converters;
using Exante.Net.Enums;
using System.Collections.Generic;

namespace Exante.Net.Converters
{
    public class ScheduleIntervalConverter : BaseConverter<ExanteScheduleInterval>
    {
        public ScheduleIntervalConverter() : this(true)
        {
        }

        public ScheduleIntervalConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<ExanteScheduleInterval, string>> Mapping => new()
            {
                new KeyValuePair<ExanteScheduleInterval, string>(ExanteScheduleInterval.PreMarket, "PreMarket"),
                new KeyValuePair<ExanteScheduleInterval, string>(ExanteScheduleInterval.MainSession, "MainSession"),
                new KeyValuePair<ExanteScheduleInterval, string>(ExanteScheduleInterval.AfterMarket, "AfterMarket"),
                new KeyValuePair<ExanteScheduleInterval, string>(ExanteScheduleInterval.Offline, "Offline"),
                new KeyValuePair<ExanteScheduleInterval, string>(ExanteScheduleInterval.Online, "Online"),
                new KeyValuePair<ExanteScheduleInterval, string>(ExanteScheduleInterval.Expired, "Expired"),
            };
    }
}