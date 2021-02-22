namespace Exante.Net.Objects
{
    public class ExanteOption
    {
        /// <summary>
        /// Option group name
        /// </summary>
        public string OptionGroupId { get; set; } = "";
        
        /// <summary>
        /// Option strike price
        /// </summary>
        public decimal StrikePrice { get; set; }
        
        /// <summary>
        /// Option right
        /// </summary>
        public string OptionRight { get; set; } = "";
    }
}