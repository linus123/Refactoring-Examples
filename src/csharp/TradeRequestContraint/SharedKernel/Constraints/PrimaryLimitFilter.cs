using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Primary Limit")]
    public class PrimaryLimitFilter : Filter
    {
        public PrimaryLimitFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;

            if (tradeFilterPreference.IsPrimaryConstraintActive)
            {
                FilteredQuantity = OriginalQuantity;
                FilterDescription = tradeRequest.PrimLimitDescription;
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);
        }
    }
}
