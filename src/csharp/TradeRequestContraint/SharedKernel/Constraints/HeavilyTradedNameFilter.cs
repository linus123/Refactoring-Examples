using System.ComponentModel;

namespace SharedKernel.Constraints
{
    [Description("Heavily Traded Name")]
    public class HeavilyTradedNameFilter : Filter
    {
        public HeavilyTradedNameFilter(decimal originalQty) : base(originalQty)
        {

        }


        public override decimal ApplyConstraint(TradeRequest tradeRequest, Profile profile)
        {
            ConstrainedQty = 0;
            if (profile.IsBlockHeavilyTradeConstraintActive)
            {
                if (tradeRequest.Block.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Block has been checked by Heavily Traded Name
                    if (tradeRequest.Block.ConstrainedByHeavilyTradedName)
                    {
                        //Constrained by Name
                        ConstrainedQty = AvailQty;
                    }
                }
                else
                {
                    //First-time Check
                    var dayNo = (int)profile.BlockHeavilyTradeDay;
                    var volumePercentage = profile.BlockHeavilyTradeVolume;
                    var tradedVolume = tradeRequest.Block.GetAccumulatedDayTradeVolume(dayNo);
                    var marketVolume = tradeRequest.Block.GetAccumulatedDayMarketVolume(dayNo);

                    if (tradedVolume > volumePercentage * marketVolume)
                    {
                        ConstrainedQty = AvailQty;
                        tradeRequest.Block.ConstrainedByHeavilyTradedName = true;
                    }
                    tradeRequest.Block.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            return CalculateConstrainedAmtAndAvailableQty(tradeRequest);

        }

    }
}
