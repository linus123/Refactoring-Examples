using System.Collections.Generic;
using System.Linq;
using SharedKernel.Filters;

namespace SharedKernel
{
    public class TradeRequest
    {
        public TradeRequest()
        {
            Filters = new List<Filter>();
        }

        public List<Filter> Filters { get; set; }

        public int TradeRequestId { get; set; }
        public string PrimaryLimitDescription { get; set; }
        public decimal OriginalCapacityQuantity { get; set; }
        public decimal AvailableCapacityQuantity { get; set; }
        public TradeSide TradeSide { get; set; }

        public decimal HoldingsQuantity { get; set; }
        public decimal EncumberedQuantity { get; set; }
        
        public Stock Stock { get; set; }
        public TradeFilterPreference TradeFilterPreference { get; set; }

        public bool IsSellOutQty(
            decimal quantity)
        {
            if (TradeSide == TradeSide.Sell)
                return quantity >= (HoldingsQuantity - EncumberedQuantity);

            return false;
        }

        public decimal CalculateUnencumberedQuantity()
        {
            var unencumberedQuantity = HoldingsQuantity - EncumberedQuantity;

            if (unencumberedQuantity < 0)
            {
                unencumberedQuantity = 0;
            }

            return unencumberedQuantity;
        }

        public void ApplyFilters(TradeFilterPreference tradeFilterPreference)
        {
            decimal availQty = this.OriginalCapacityQuantity;


            if (tradeFilterPreference.IsPrimaryFilterActive)
            {
                availQty = ApplyConstraint(tradeFilterPreference, new PrimaryLimitFilter(availQty));
            }

            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                availQty = ApplyConstraint(tradeFilterPreference, new HeavilyTradedNameFilter(availQty));
            }

            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                availQty = ApplyConstraint(tradeFilterPreference, new EncumberedFilter(availQty));
            }

            //Remove Zero Constrained Qty Filters
            Filters = Filters.Where(c => c.FilteredQuantity != 0).ToList();

            AvailableCapacityQuantity = availQty;
        }

        public decimal ApplyConstraint(TradeFilterPreference tradeFilterPreference, Filter filter)
        {
            Filters.Add(filter);

            return filter.ApplyFilter(this, tradeFilterPreference);
        }
    }
}