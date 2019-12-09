using System.Collections.Generic;
using System.Linq;
using SharedKernel.Filters;

namespace SharedKernel
{
    public class TradeRequest
    {
        public TradeRequest()
        {
            Filters = new List<FilterModel>();
        }

        public List<FilterModel> Filters { get; set; }

        public int TradeRequestId { get; set; }
        public decimal PrimaryLimit { get; set; }
        public string PrimaryLimitDescription { get; set; }
        public decimal OriginalCapacityQuantity { get; set; }
        public decimal AvailableCapacityQuantity { get; set; }
        public TradeSide TradeSide { get; set; }

        public decimal HoldingsQuantity { get; set; }
        public decimal EncumberedQuantity { get; set; }
        
        public Stock Stock { get; set; }
        public TradeFilterPreference TradeFilterPreference { get; set; }

        public decimal GetOriginalCapacityAmount()
        {
            return OriginalCapacityQuantity * Stock.PriceInUsd;
        }

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
                var filter = new PrimaryLimitFilter(availQty);

                availQty = filter.ApplyFilter(this, tradeFilterPreference);

                Filters.Add(filter.CreateModel());

            }

            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                var filter = new HeavilyTradedNameFilter(availQty);

                availQty = filter.ApplyFilter(this, tradeFilterPreference);

                Filters.Add(filter.CreateModel());
            }

            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                var filter = new EncumberedFilter(availQty);

                availQty = filter.ApplyFilter(this, tradeFilterPreference);

                Filters.Add(filter.CreateModel());
            }

            //Remove Zero Constrained Qty Filters
            Filters = Filters.Where(c => c.FilteredQuantity != 0).ToList();

            AvailableCapacityQuantity = availQty;
        }

        public bool ShouldPrimaryConstraintBeApplied(TradeFilterPreference tradeFilterPreference)
        {
            if (!tradeFilterPreference.IsPrimaryFilterActive) return false;

            if (TradeSide == TradeSide.Buy)
            {
                return Stock.IsSharePriceWithBufferGreaterThan(PrimaryLimit, tradeFilterPreference.CapacityPrimaryLimitBuy);
            }
            else // Sell
            {
                return Stock.IsSharePriceWithBufferLessThan(PrimaryLimit, tradeFilterPreference.CapacityPrimaryLimitSell);
            }
        }
    }
}