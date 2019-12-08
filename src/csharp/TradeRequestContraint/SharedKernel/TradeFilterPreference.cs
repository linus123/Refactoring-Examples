namespace SharedKernel
{
    public class TradeFilterPreference
    {
        public bool IsPrimaryFilterActive { get; set; }
        public decimal CapacityPrimaryLimitBuy { get; set; }
        public decimal CapacityPrimaryLimitSell { get; set; }

        public bool IsCapacityEncumberedSharesFilterActive { get; set; }

        public bool IsHeavilyTradeFilterActive { get; set; }
        public int StockHeavilyTradeDay { get; set; }
        public decimal StockHeavilyTradeVolume { get; set; }
    }
}