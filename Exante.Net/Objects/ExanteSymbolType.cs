using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;

namespace Exante.Net.Objects
{
    public class ExanteSymbolTypeId
    {
        [JsonConverter(typeof(SymbolTypeConverter))]
        public ExanteSymbolType Id { get; set; }
    }
}