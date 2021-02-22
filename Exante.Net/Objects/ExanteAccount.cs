using Exante.Net.Converters;
using Exante.Net.Enums;
using Newtonsoft.Json;

namespace Exante.Net.Objects
{
    public class ExanteAccount
    {
        /// <summary>
        /// Account ID
        /// </summary>
        public string AccountId { get; set; } = "";

        /// <summary>
        /// Account status
        /// </summary>
        [JsonConverter(typeof(AccountStatusConverter))]
        public ExanteAccountStatus Status { get; set; }
    }
}