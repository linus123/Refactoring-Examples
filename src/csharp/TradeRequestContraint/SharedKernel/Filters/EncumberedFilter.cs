namespace SharedKernel.Filters
{
    public class EncumberedFilter : Filter
    {

        public EncumberedFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public decimal FilteredQuantity { get; set; }
        public decimal FilteredAmount { get; set; }

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(OriginalQuantity))
                {
                    FilteredQuantity = OriginalQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            FilteredAmount = FilteredQuantity * tradeRequest.Stock.PriceInUsd;

            AvailableQuantity = OriginalQuantity - FilteredQuantity;

            if (AvailableQuantity < 0)
            {
                AvailableQuantity = 0;
            }

            return AvailableQuantity;
        }

        public FilterModel CreateModel()
        {
            return new FilterModel()
            {
                FilterType = "Encumbered",
                FilteredAmount = this.FilteredAmount,
                FilteredQuantity = this.FilteredQuantity,
                OriginalQuantity = this.OriginalQuantity,
                AvailableQuantity = this.AvailableQuantity,
                FilterDescription = null,
                IsApplied = true,
            };
        }

    }
}