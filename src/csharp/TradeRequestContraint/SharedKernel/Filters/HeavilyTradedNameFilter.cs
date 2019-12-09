namespace SharedKernel.Filters
{
    public class HeavilyTradedNameFilter
    {
        private decimal _filteredQuantity;
        private decimal _filteredAmount;

        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            var availableQuantity = startingQuantity;

            _filteredQuantity = 0;
            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                if (tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Stock has been checked by Heavily Traded Name
                    if (tradeRequest.Stock.ConstrainedByHeavilyTradedName)
                    {
                        //Constrained by Name
                        _filteredQuantity = availableQuantity;
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
                        _filteredQuantity = availableQuantity;
                        tradeRequest.Stock.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            _filteredAmount = _filteredQuantity * tradeRequest.Stock.PriceInUsd;

            availableQuantity = startingQuantity - _filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Heavily Traded Name",
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
