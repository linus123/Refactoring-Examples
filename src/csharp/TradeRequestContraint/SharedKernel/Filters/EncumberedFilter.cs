namespace SharedKernel.Filters
{
    public class EncumberedFilter : Filter
    {

        public EncumberedFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override string GetFilterType()
        {
            return "Encumbered";
        }

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
    }
}