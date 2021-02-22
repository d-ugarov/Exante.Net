using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteInstrumentGroup
    {
        /// <summary>
        /// Group id
        /// </summary>
        public string Group { get; set; } = "";
        
        /// <summary>
        /// Group title
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Exchange id where the group is traded
        /// </summary>
        public string? Exchange { get; set; }

        /// <summary>
        /// List of symbol types in the group
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(SymbolTypeConverter))]
        public IEnumerable<ExanteSymbolType> Types { get; set; } = new List<ExanteSymbolType>();
    }
}