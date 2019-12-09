namespace SharedKernel.Filters
{
    public class EncumberedFilter
    {
        private decimal _filteredQuantity;
        private decimal _filteredAmount;

        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(startingQuantity))
                {
                    _filteredQuantity = startingQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            var availableQuantity = startingQuantity - _filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Encumbered",
                FilteredAmount = _filteredAmount,
                FilteredQuantity = _filteredQuantity,
                OriginalQuantity = startingQuantity,
                AvailableQuantity = availableQuantity,
                FilterDescription = null,
                IsApplied = true,
            };
        }
    }
}