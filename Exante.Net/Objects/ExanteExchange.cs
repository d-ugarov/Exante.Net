namespace Exante.Net.Objects
{
    public class ExanteExchange
    {
        /// <summary>
        /// Exchange internal id
        /// </summary>
        public string Id { get; set; } = "";
        
        /// <summary>
        /// Full exchange name
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Exchange country
        /// </summary>
        public string? Country { get; set; }
    }
}