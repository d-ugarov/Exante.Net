using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteSchedule
    {
        /// <summary>
        /// Instrument schedule intervals
        /// </summary>
        public IEnumerable<ExanteInterval> Intervals { get; set; } = new List<ExanteInterval>();
    }

    public class ExanteInterval
    {
        /// <summary>
        /// Trading session name
        /// </summary>
        [JsonConverter(typeof(ScheduleIntervalConverter))]
        public ExanteScheduleInterval Name { get; set; }

        /// <summary>
        /// Trading session period
        /// </summary>
        public ExantePeriod Period { get; set; } = new();
        
        public ExanteOrderTypes? OrderTypes { get; set; }
    }
}