namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter : Filter
    {
        public PrimaryLimitFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public decimal FilteredQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                FilteredQuantity = OriginalQuantity;
                FilterDescription = tradeRequest.PrimaryLimitDescription;
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
                FilterType = "Primary Limit",
                FilteredAmount = this.FilteredAmount,
                FilteredQuantity = this.FilteredQuantity,
                OriginalQuantity = this.OriginalQuantity,
                AvailableQuantity = this.AvailableQuantity,
                FilterDescription = this.FilterDescription,
                IsApplied = true,
            };
        }
    }
}
