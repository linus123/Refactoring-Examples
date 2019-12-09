namespace SharedKernel.Filters
{
    public class HeavilyTradedNameFilter
    {
        public HeavilyTradedNameFilter(decimal startingQuantity)
        {
            _startingQuantity = startingQuantity;
            _availableQuantity = startingQuantity;
        }

        private readonly decimal _startingQuantity;
        private decimal _availableQuantity;

        private decimal _filteredQuantity;
        private decimal _filteredAmount;

        public FilterModel ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            _filteredQuantity = 0;
            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                if (tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Stock has been checked by Heavily Traded Name
                    if (tradeRequest.Stock.ConstrainedByHeavilyTradedName)
                    {
                        //Constrained by Name
                        _filteredQuantity = _availableQuantity;
                    }
                }
                else
                {
                    //First-time Check
                    var dayNo = (int)tradeFilterPreference.StockHeavilyTradeDay;
                    var volumePercentage = tradeFilterPreference.StockHeavilyTradeVolume;
                    var tradedVolume = tradeRequest.Stock.GetAccumulatedDayTradeVolume(dayNo);
                    var marketVolume = tradeRequest.Stock.GetAccumulatedDayMarketVolume(dayNo);

                    if (tradedVolume > volumePercentage * marketVolume)
                    {
                        _filteredQuantity = _availableQuantity;
                        tradeRequest.Stock.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            _availableQuantity = _startingQuantity - _filteredQuantity;

            if (_availableQuantity < 0)
            {
                _availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Heavily Traded Name",
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
