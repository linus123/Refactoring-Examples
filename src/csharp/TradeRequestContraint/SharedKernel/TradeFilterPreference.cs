namespace SharedKernel
{
    public class TradeFilterPreference
    {
        public bool IsPrimaryConstraintActive { get; set; }

        public bool IsCapacityEncumberedSharesConstraintActive { get; set; }

        public bool IsBlockHeavilyTradeConstraintActive { get; set; }

        public int BlockHeavilyTradeDay { get; set; }

        public decimal BlockHeavilyTradeVolume { get; set; }
    }
}