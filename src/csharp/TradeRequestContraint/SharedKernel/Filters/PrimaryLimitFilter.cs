namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter
    {
        private decimal _filteredQuantity;
        private decimal _filteredAmount;
        private string _filterDescription;

        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                _filteredQuantity = startingQuantity;
                _filterDescription = tradeRequest.PrimaryLimitDescription;
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            var availableQuantity = startingQuantity - _filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Primary Limit",
                FilteredAmount = _filteredAmount,
                FilteredQuantity = _filteredQuantity,
                OriginalQuantity = startingQuantity,
                AvailableQuantity = availableQuantity,
                FilterDescription = _filterDescription,
                IsApplied = true,
            };
        }
    }
}
