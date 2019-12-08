using System.ComponentModel;

namespace SharedKernel.Filters
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

            if (tradeFilterPreference.IsPrimaryFilterActive)
            {
                FilteredQuantity = OriginalQuantity;
                FilterDescription = tradeRequest.PrimaryLimitDescription;
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);
        }
    }
}
