using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Encumbered")]
    public class EncumberedFilter : Filter
    {

        public EncumberedFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override decimal ApplyFilter(TradeRequest tradeRequest, Profile profile)
        {
            FilteredQuantity = 0;
            if (profile.IsCapacityEncumberedSharesConstraintActive)
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