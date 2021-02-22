namespace Exante.Net.Objects
{
    public class ExanteSpecification
    {
        /// <summary>
        /// Instrument price unit
        /// </summary>
        public decimal PriceUnit { get; set; }
        
        /// <summary>
        /// Instrument units name
        /// </summary>
        public string? Units { get; set; }
        
        /// <summary>
        /// Instrument lot size value
        /// </summary>
        public decimal LotSize { get; set; }
        
        /// <summary>
        /// Instrument leverage rate value
        /// </summary>
        public decimal Leverage { get; set; }
        
        /// <summary>
        /// Instrument contract multiplier
        /// </summary>
        public decimal ContractMultiplier { get; set; }
    }
}