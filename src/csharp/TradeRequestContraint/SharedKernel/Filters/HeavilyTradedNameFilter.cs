namespace SharedKernel.Filters
{
    public class HeavilyTradedNameFilter : Filter
    {
        public HeavilyTradedNameFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public decimal FilteredQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }

        public decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;
            if (tradeFilterPreference.IsHeavilyTradeFilterActive)
            {
                if (tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Stock has been checked by Heavily Traded Name
                    if (tradeRequest.Stock.ConstrainedByHeavilyTradedName)
                    {
                        //Constrained by Name
                        FilteredQuantity = AvailableQuantity;
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
                        FilteredQuantity = AvailableQuantity;
                        tradeRequest.Stock.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            FilteredAmount = FilteredQuantity * tradeRequest.Stock.PriceInUsd;

            AvailableQuantity = OriginalQuantity - FilteredQuantity;

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
                FilterType = "Heavily Traded Name",
                FilteredAmount = this.FilteredAmount,
                FilteredQuantity = this.FilteredQuantity,
                OriginalQuantity = this.OriginalQuantity,
                AvailableQuantity = this.AvailableQuantity,
                FilterDescription = this.FilterDescription,
                IsApplied = true,
            };
        }

    }
}
