namespace Exante.Net.Objects
{
    public class ExanteDailyChange
    {
        /// <summary>
        /// Symbol id
        /// </summary>
        public string SymbolId { get; set; } = "";
        
        /// <summary>
        /// Absolute daily change of the price at the moment of request
        /// </summary>
        public decimal? DailyChange { get; set; }
        
        /// <summary>
        /// Previous session close price
        /// </summary>
        public decimal? LastSessionClosePrice { get; set; }
    }
}