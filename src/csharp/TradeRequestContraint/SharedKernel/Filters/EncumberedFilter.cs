namespace SharedKernel.Filters
{
    public class EncumberedFilter
    {
        public EncumberedFilter(decimal originalQuantity)
        {
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
        }

        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }

        private decimal _filteredQuantity;
        private decimal _filteredAmount;

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(OriginalQuantity))
                {
                    _filteredQuantity = OriginalQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            AvailableQuantity = OriginalQuantity - _filteredQuantity;

            if (AvailableQuantity < 0)
            {
                AvailableQuantity = 0;
            }

            return AvailableQuantity;
        }

        public FilterModel CreateModel()
        {
            return new FilterModel()
            {
                FilterType = "Encumbered",
                FilteredAmount = this._filteredAmount,
                FilteredQuantity = this._filteredQuantity,
                OriginalQuantity = this.OriginalQuantity,
                AvailableQuantity = this.AvailableQuantity,
                FilterDescription = null,
                IsApplied = true,
            };
        }

    }
}