using System.ComponentModel;

namespace SharedKernel.Filters
{
    [Description("Encumbered")]
    public class EncumberedFilter : Filter
    {

        public EncumberedFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesConstraintActive)
            {
                if (tradeRequest.IsSellOutQty(OriginalQuantity))
                {
                    FilteredQuantity = OriginalQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);
        }
    }
}