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
            if (IsSell())
            {
                var encumberedQuantity = HoldingsQuantity - EncumberedQuantity;

                return quantity >= encumberedQuantity;
            }

            return false;
        }

        private bool IsSell()
        {
            return TradeSide == TradeSide.Sell;
        }

        public bool IsBuy()
        {
            return TradeSide == TradeSide.Buy;
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
                var model = new PrimaryLimitFilter()
                    .CreateModel(availQty, this, tradeFilterPreference);

                availQty = model.AvailableQuantity;

                Filters.Add(model);

            }

            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                var model = new HeavilyTradedNameFilter()
                    .CreateModel(availQty, this, tradeFilterPreference);

                availQty = model.AvailableQuantity;

                Filters.Add(model);
            }

            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                var model = new EncumberedFilter()
                    .CreateModel(availQty, this, tradeFilterPreference);

                availQty = model.AvailableQuantity;

                Filters.Add(model);
            }

            //Remove Zero Constrained Qty Filters
            Filters = Filters.Where(c => c.FilteredQuantity != 0).ToList();

            AvailableCapacityQuantity = availQty;
        }
    }
}