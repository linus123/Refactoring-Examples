namespace SharedKernel
{
    public class Stock
    {
        public string StockId { get; set; }
        public decimal PriceInUsd { get; set; }
        public bool IsHeavilyTradedNameConstraintChecked { get; set; }
        public bool ConstrainedByHeavilyTradedName { get; set; }

        private decimal[] _tradeVolumes;
        private decimal[] _marketVolumes;

        public void SetVolumes(
            decimal[] tradeVolumes,
            decimal[] marketVolumes)
        {
            _tradeVolumes = tradeVolumes;
            _marketVolumes = marketVolumes;
        }

        public decimal GetAccumulatedDayTradeVolume(
            int dayNumber)
        {
            return _tradeVolumes[dayNumber];
        }

        public decimal GetAccumulatedDayMarketVolume(
            int dayNumber)
        {
            return _marketVolumes[dayNumber];
        }
    }
}