using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Primary Limit")]
    public class PrimaryLimitFilter : Filter
    {
        public PrimaryLimitFilter(decimal originalQty) : base(originalQty)
        {

        }

        public override decimal ApplyConstraint(TradeRequest tradeRequest, Profile profile)
        {
            ConstrainedQty = 0;

            if (profile.IsPrimaryConstraintActive)
            {
                ConstrainedQty = OriginalQty;
                ConstraintDescription = tradeRequest.PrimLimitDescription;
            }

            return CalculateConstrainedAmtAndAvailableQty(tradeRequest);
        }
    }
}
