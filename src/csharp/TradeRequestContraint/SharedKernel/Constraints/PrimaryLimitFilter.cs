using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Primary Limit")]
    public class PrimaryLimitFilter : Filter
    {
        public PrimaryLimitFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override decimal ApplyFilter(TradeRequest tradeRequest, Profile profile)
        {
            FilteredQuantity = 0;

            if (profile.IsPrimaryConstraintActive)
            {
                FilteredQuantity = OriginalQuantity;
                ConstraintDescription = tradeRequest.PrimLimitDescription;
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);
        }
    }
}
