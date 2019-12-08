namespace SharedKernel
{
    public class TradeFilterPreference
    {
        public bool IsPrimaryFilterActive { get; set; }

        public bool IsCapacityEncumberedSharesFilterActive { get; set; }

        public bool IsHeavilyTradeFilterActive { get; set; }

        public int BlockHeavilyTradeDay { get; set; }

        public decimal BlockHeavilyTradeVolume { get; set; }
    }
}