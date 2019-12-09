namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter
    {
        public PrimaryLimitFilter(decimal originalQuantity)
        {
            _originalQuantity = originalQuantity;
            _availableQuantity = originalQuantity;
        }

        private readonly decimal _originalQuantity;
        private decimal _availableQuantity;

        private decimal _filteredQuantity;
        private decimal _filteredAmount;
        private string _filterDescription;

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                _filteredQuantity = _originalQuantity;
                _filterDescription = tradeRequest.PrimaryLimitDescription;
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            _availableQuantity = _originalQuantity - _filteredQuantity;

            if (_availableQuantity < 0)
            {
                _availableQuantity = 0;
            }

            return _availableQuantity;
        }

        public FilterModel CreateModel()
        {
            return new FilterModel()
            {
                FilterType = "Primary Limit",
                FilteredAmount = _filteredAmount,
                FilteredQuantity = _filteredQuantity,
                OriginalQuantity = _originalQuantity,
                AvailableQuantity = _availableQuantity,
                FilterDescription = _filterDescription,
                IsApplied = true,
            };
        }
    }
}
