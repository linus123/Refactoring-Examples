﻿using System.ComponentModel;

namespace SharedKernel.Filters
{
    [Description("Heavily Traded Name")]
    public class HeavilyTradedNameFilter : Filter
    {
        public HeavilyTradedNameFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }


        public override decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
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
                        FilteredQuantity = AvailQuantity;
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
                        FilteredQuantity = AvailQuantity;
                        tradeRequest.Stock.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);

        }

    }
}