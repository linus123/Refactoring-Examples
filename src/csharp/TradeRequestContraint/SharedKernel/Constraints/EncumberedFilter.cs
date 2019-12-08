using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Encumbered")]
    public class EncumberedFilter : Filter
    {

        public EncumberedFilter(decimal originalQty) : base(originalQty)
        {

        }

        public override decimal ApplyConstraint(TradeRequest tradeRequest, Profile profile)
        {
            ConstrainedQty = 0;
            if (profile.IsCapacityEncumberedSharesConstraintActive)
            {
                if (tradeRequest.IsSellOutQty(OriginalQty))
                {
                    ConstrainedQty = OriginalQty - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            return CalculateConstrainedAmtAndAvailableQty(tradeRequest);
        }
    }
}