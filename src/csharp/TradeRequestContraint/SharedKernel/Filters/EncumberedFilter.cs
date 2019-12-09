namespace SharedKernel.Filters
{
    public class EncumberedFilter
    {
        public EncumberedFilter(decimal startingQuantity)
        {
            _startingQuantity = startingQuantity;
            _availableQuantity = startingQuantity;
        }

        private readonly decimal _startingQuantity;
        private decimal _availableQuantity;

        private decimal _filteredQuantity;
        private decimal _filteredAmount;

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(_startingQuantity))
                {
                    _filteredQuantity = _startingQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            _availableQuantity = _startingQuantity - _filteredQuantity;

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
                OriginalQuantity = _startingQuantity,
                AvailableQuantity = _availableQuantity,
                FilterDescription = null,
                IsApplied = true,
            };
        }

    }
}