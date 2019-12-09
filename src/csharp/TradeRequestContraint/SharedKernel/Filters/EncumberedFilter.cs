﻿namespace SharedKernel.Filters
{
    public class EncumberedFilter : Filter
    {

        public EncumberedFilter(decimal originalQuantity) : base(originalQuantity)
        {

        }

        public override string GetFilterType()
        {
            return "Encumbered";
        }

        public override decimal ApplyFilter(TradeRequest tradeRequest, TradeFilterPreference tradeFilterPreference)
        {
            FilteredQuantity = 0;
            if (tradeFilterPreference.IsCapacityEncumberedSharesFilterActive)
            {
                if (tradeRequest.IsSellOutQty(OriginalQuantity))
                {
                    FilteredQuantity = OriginalQuantity - tradeRequest.CalculateUnencumberedQuantity();
                }
            }

            return CalculateFilteredAmountAndAvailableQuantity(tradeRequest);
        }
    }
}