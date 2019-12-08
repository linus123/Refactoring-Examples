using System.ComponentModel;

namespace SharedKernel
{
    [Description("Primary Limit")]
    public class PrimaryLimitConstraint : Constraint
    {
        public PrimaryLimitConstraint(decimal originalQty) : base(originalQty)
        {

        }

        public override decimal ApplyConstraint(OrderCapacity orderCapacity, Profile profile)
        {
            ConstrainedQty = 0;

            if (profile.IsPrimaryConstraintActive)
            {
                ConstrainedQty = OriginalQty;
                ConstraintDescription = orderCapacity.PrimLimitDescription;
            }

            return CalculateConstrainedAmtAndAvailableQty(orderCapacity);
        }
    }
}
