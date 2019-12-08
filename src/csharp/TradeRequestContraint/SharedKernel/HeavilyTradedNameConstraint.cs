using System.ComponentModel;

namespace SharedKernel
{
    [Description("Heavily Traded Name")]
    public class HeavilyTradedNameConstraint : Constraint
    {
        public HeavilyTradedNameConstraint(decimal originalQty) : base(originalQty)
        {

        }


        public override decimal ApplyConstraint(OrderCapacity orderCapacity, Profile profile)
        {
            ConstrainedQty = 0;
            if (profile.IsBlockHeavilyTradeConstraintActive)
            {
                if (orderCapacity.Block.IsHeavilyTradedNameConstraintChecked == true)
                {
                    //Block has been checked by Heavily Traded Name
                    if (orderCapacity.Block.ConstrainedByHeavilyTradedName)
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
                    var tradedVolume = orderCapacity.Block.GetAccumulatedDayTradeVolume(dayNo);
                    var marketVolume = orderCapacity.Block.GetAccumulatedDayMarketVolume(dayNo);

                    if (tradedVolume > volumePercentage * marketVolume)
                    {
                        ConstrainedQty = AvailQty;
                        orderCapacity.Block.ConstrainedByHeavilyTradedName = true;
                    }
                    orderCapacity.Block.IsHeavilyTradedNameConstraintChecked = true;

                }
            }

            return CalculateConstrainedAmtAndAvailableQty(orderCapacity);

        }

    }
}
