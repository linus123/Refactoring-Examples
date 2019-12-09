namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter
    {
        public PrimaryLimitFilter(decimal startingQuantity)
        {
            _startingQuantity = startingQuantity;
            _availableQuantity = startingQuantity;
        }

        private readonly decimal _startingQuantity;
        private decimal _availableQuantity;

        private decimal _filteredQuantity;
        private decimal _filteredAmount;
        private string _filterDescription;

        public FilterModel ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                _filteredQuantity = _startingQuantity;
                _filterDescription = tradeRequest.PrimaryLimitDescription;
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            _availableQuantity = _startingQuantity - _filteredQuantity;

            if (_availableQuantity < 0)
            {
                _availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Primary Limit",
                FilteredAmount = _filteredAmount,
                FilteredQuantity = _filteredQuantity,
                OriginalQuantity = _startingQuantity,
                AvailableQuantity = _availableQuantity,
                FilterDescription = _filterDescription,
                IsApplied = true,
            };
        }
    }
}
