using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Encumbered")]
    public class EncumberedConstraint : Constraint
    {

        public EncumberedConstraint(decimal originalQty) : base(originalQty)
        {

        }

        public override decimal ApplyConstraint(OrderCapacity orderCapacity, Profile profile)
        {
            ConstrainedQty = 0;
            if (profile.IsCapacityEncumberedSharesConstraintActive)
            {
                if (orderCapacity.IsSellOutQty(OriginalQty))
                {
                    ConstrainedQty = OriginalQty - orderCapacity.CalculateUnencumberedQuantity();
                }
            }

            return CalculateConstrainedAmtAndAvailableQty(orderCapacity);
        }
    }
}