namespace SharedKernel.Filters
{
    public class HeavilyTradedNameFilter
    {
        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            var availableQuantity = startingQuantity;

            decimal filteredQuantity = 0;

            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                if (tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Stock has been checked by Heavily Traded Name
                    if (tradeRequest.Stock.ConstrainedByHeavilyTradedName)
                    {
                        //Constrained by Name
                        filteredQuantity = availableQuantity;
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
                        filteredQuantity = availableQuantity;
                        tradeRequest.Stock.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            var filteredAmount = filteredQuantity * tradeRequest.Stock.PriceInUsd;

            availableQuantity = startingQuantity - filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Heavily Traded Name",
                FilteredAmount = filteredAmount,
                FilteredQuantity = filteredQuantity,
                OriginalQuantity = startingQuantity,
                AvailableQuantity = availableQuantity,
                FilterDescription = null,
                IsApplied = true,
            };

        }
    }
}
