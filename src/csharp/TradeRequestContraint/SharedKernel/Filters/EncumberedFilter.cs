namespace SharedKernel.Filters
{
    public class EncumberedFilter
    {
        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            decimal filteredQuantity = 0;

            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(startingQuantity))
                {
                    filteredQuantity = startingQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            var filteredAmount = filteredQuantity * tradeRequest.Stock.PriceInUsd;

            var availableQuantity = startingQuantity - filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Encumbered",
                FilteredAmount = filteredAmount,
                FilteredQuantity = filteredQuantity,
                OriginalQuantity = startingQuantity,
                AvailableQuantity = availableQuantity,
                FilterDescription = null
            };
        }
    }
}