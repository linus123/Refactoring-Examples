using System;
using System.ComponentModel;

namespace SharedKernel.Constraints
{
    public class Filter
    {
        public Filter()
        {

        }

        public Filter(decimal originalQty)
        {
            ConstraintType = GetClassDescription(this.GetType());
            OriginalQty = originalQty;
            AvailQty = originalQty;
            IsApplied = true;
        }

        public virtual decimal ApplyConstraint(TradeRequest tradeRequest, Profile profile)
        { return OriginalQty; }

        public decimal ConstrainedQty { get; set; }
        public string ConstraintType { get; set; }
        public decimal OriginalQty { get; set; }
        public decimal AvailQty { get; set; }
        public decimal ConstrainedAmt { get; set; }
        public string ConstraintDescription { get; set; }

        public bool IsApplied { get; set; }

        public decimal CalculateConstrainedAmtAndAvailableQty(TradeRequest tradeRequest)
        {
            ConstrainedAmt = ConstrainedQty * tradeRequest.Block.PriceInUsd;

            AvailQty = OriginalQty - ConstrainedQty;

            if (AvailQty < 0)
            {
                AvailQty = 0;
            }

            return AvailQty;
        }

        private static string GetClassDescription(Type type)
        {
            var descriptions = (DescriptionAttribute[])
                type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }
    }
}
