namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter
    {
        public PrimaryLimitFilter(decimal originalQuantity)
        {
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
        }

        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }

        private decimal _filteredQuantity;
        private decimal _filteredAmount;
        private string _filterDescription;

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;

            if (tradeRequest.ShouldPrimaryConstraintBeApplied(tradeFilterPreference))
            {
                _filteredQuantity = OriginalQuantity;
                _filterDescription = tradeRequest.PrimaryLimitDescription;
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
                FilterType = "Primary Limit",
                FilteredAmount = this._filteredAmount,
                FilteredQuantity = this._filteredQuantity,
                OriginalQuantity = this.OriginalQuantity,
                AvailableQuantity = this.AvailableQuantity,
                FilterDescription = this._filterDescription,
                IsApplied = true,
            };
        }
    }
}
