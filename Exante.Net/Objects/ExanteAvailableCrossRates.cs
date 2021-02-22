using System.Collections.Generic;

namespace Exante.Net.Objects
{
    public class ExanteAvailableCrossRates
    {
        /// <summary>
        /// List of available currencies
        /// </summary>
        public IEnumerable<string> Currencies { get; set; } = new List<string>();
    }
}