namespace SharedKernel.Filters
{
    public class Filter
    {
        public Filter(decimal originalQuantity)
        {
            FilterType = GetFilterType();
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
            IsApplied = true;
        }

        public virtual string GetFilterType()
        {
            return null;
        }

        public virtual decimal ApplyFilter(
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            return OriginalQuantity;
        }

        public decimal FilteredQuantity { get; set; }
        public string FilterType { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }

        public bool IsApplied { get; set; }

        public decimal CalculateFilteredAmountAndAvailableQuantity(TradeRequest tradeRequest)
        {
            FilteredAmount = FilteredQuantity * tradeRequest.Stock.PriceInUsd;

            AvailableQuantity = OriginalQuantity - FilteredQuantity;

            if (AvailableQuantity < 0)
            {
                AvailableQuantity = 0;
            }

            return AvailableQuantity;
        }
    }
}
