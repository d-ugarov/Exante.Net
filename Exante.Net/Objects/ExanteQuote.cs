namespace Exante.Net.Objects
{
    public class ExanteQuote
    {
        /// <summary>
        /// Quote value of this level
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Quantity value of this level
        /// </summary>
        public decimal Size { get; set; }
    }
}