namespace SharedKernel.Filters
{
    public class EncumberedFilter
    {
        public EncumberedFilter(decimal originalQuantity)
        {
            _originalQuantity = originalQuantity;
            _availableQuantity = originalQuantity;
        }

        private readonly decimal _originalQuantity;
        private decimal _availableQuantity;

        private decimal _filteredQuantity;
        private decimal _filteredAmount;

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(_originalQuantity))
                {
                    _filteredQuantity = _originalQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
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
                FilterType = "Encumbered",
                FilteredAmount = _filteredAmount,
                FilteredQuantity = _filteredQuantity,
                OriginalQuantity = _originalQuantity,
                AvailableQuantity = _availableQuantity,
                FilterDescription = null,
                IsApplied = true,
            };
        }

    }
}