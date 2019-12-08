using System.ComponentModel;

namespace SharedKernel.Constraints
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
            if (tradeFilterPreference.IsBlockHeavilyTradeConstraintActive)
            {
                if (tradeRequest.Block.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Block has been checked by Heavily Traded Name
                    if (tradeRequest.Block.ConstrainedByHeavilyTradedName)
                    {
                        //Constrained by Name
                        FilteredQuantity = AvailQuantity;
                    }
                }
                else
                {
                    //First-time Check
                    var dayNo = (int)tradeFilterPreference.BlockHeavilyTradeDay;
                    var volumePercentage = tradeFilterPreference.BlockHeavilyTradeVolume;
                    var tradedVolume = tradeRequest.Block.GetAccumulatedDayTradeVolume(dayNo);
                    var marketVolume = tradeRequest.Block.GetAccumulatedDayMarketVolume(dayNo);

                    if (tradedVolume > volumePercentage * marketVolume)
                    {
                        FilteredQuantity = AvailQuantity;
                        tradeRequest.Block.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Block.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);

        }

    }
}
