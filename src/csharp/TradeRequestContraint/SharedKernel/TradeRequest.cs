using System.Collections.Generic;
using System.Linq;
using SharedKernel.Constraints;

namespace SharedKernel
{
    public class TradeRequest
    {
        public TradeRequest()
        {
            Constraints = new List<Filter>();
        }

        public List<Filter> Constraints { get; set; }

        public int OrderId { get; set; }
        public string PrimLimitDescription { get; set; }
        public decimal OriginalCapacityQuantity { get; set; }
        public decimal AvailCapacityQty { get; set; }
        public TradeSide TradeSide { get; set; }

        public decimal HoldingsQty { get; set; }
        public decimal EncumberedQty { get; set; }
        
        public Block Block { get; set; }
        public Profile Profile { get; set; }

        public bool IsSellOutQty(
            decimal quantity)
        {
            if (TradeSide == TradeSide.Sell)
                return quantity >= (HoldingsQty - EncumberedQty);

            return false;
        }

        public decimal CalculateUnencumberedQuantity()
        {
            var unencumberedQuantity = HoldingsQty - EncumberedQty;

            if (unencumberedQuantity < 0)
            {
                unencumberedQuantity = 0;
            }

            return unencumberedQuantity;
        }

        public void ApplyConstraints(Profile profile)
        {
            decimal availQty = this.OriginalCapacityQuantity;


            if (profile.IsPrimaryConstraintActive)
            {
                availQty = ApplyConstraint(profile, new PrimaryLimitFilter(availQty));
            }

            if (profile.IsBlockHeavilyTradeConstraintActive)
            {
                availQty = ApplyConstraint(profile, new HeavilyTradedNameFilter(availQty));
            }

            if (profile.IsCapacityEncumberedSharesConstraintActive)
            {
                availQty = ApplyConstraint(profile, new EncumberedFilter(availQty));
            }

            //Remove Zero Constrained Qty Constraints
            Constraints = Constraints.Where(c => c.ConstrainedQty != 0).ToList();

            AvailCapacityQty = availQty;
        }

        public decimal ApplyConstraint(Profile profile, Filter filter)
        {
            Constraints.Add(filter);

            return filter.ApplyConstraint(this, profile);
        }
    }
}