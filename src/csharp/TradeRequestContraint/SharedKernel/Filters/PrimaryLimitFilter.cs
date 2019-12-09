using System.ComponentModel;

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

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);
        }
    }
}
