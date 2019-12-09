namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter
    {
        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            decimal filteredQuantity = 0;
            string filterDescription = null;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                filteredQuantity = startingQuantity;
                filterDescription = tradeRequest.PrimaryLimitDescription;
            }

            var filteredAmount = filteredQuantity * tradeRequest.Stock.PriceInUsd;

            var availableQuantity = startingQuantity - filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Primary Limit",
                FilteredAmount = filteredAmount,
                FilteredQuantity = filteredQuantity,
                OriginalQuantity = startingQuantity,
                AvailableQuantity = availableQuantity,
                FilterDescription = filterDescription,
            };
        }
    }
}
