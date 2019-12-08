namespace SharedKernel
{
    public class Block
    {
        public decimal PriceInUsd { get; set; }
        public bool IsHeavilyTradedNameConstraintChecked { get; set; }
        public bool ConstrainedByHeavilyTradedName { get; set; }

        public decimal GetAccumulatedDayTradeVolume(
            int dayNumber)
        {
            return 0m;
        }

        public decimal GetAccumulatedDayMarketVolume(
            int dayNumber)
        {
            return 0m;
        }
    }
}