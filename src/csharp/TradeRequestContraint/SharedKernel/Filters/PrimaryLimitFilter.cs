namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter : Filter
    {
        public PrimaryLimitFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override string GetFilterType()
        {
            return "Primary Limit";
        }

        public override decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                FilteredQuantity = OriginalQuantity;
                FilterDescription = tradeRequest.PrimaryLimitDescription;
            }

            FilteredAmount = FilteredQuantity * tradeRequest.Stock.PriceInUsd;

            AvailableQuantity = OriginalQuantity - FilteredQuantity;

            if (AvailableQuantity < 0)
            {
                AvailableQuantity = 0;
            }

            return AvailableQuantity;
        }
    }
}
