namespace SharedKernel.Filters
{
    public class PrimaryLimitFilter
    {
        public FilterModel CreateModel(
            decimal startingQuantity,
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            bool ret;

            if (!tradeFilterPreference.IsPrimaryFilterActive)
                ret = false;

            else
            {
                if (tradeRequest.IsBuy())
                {
                    ret = tradeRequest.Stock.IsSharePriceWithBufferGreaterThan(tradeRequest.PrimaryLimit, tradeFilterPreference.CapacityPrimaryLimitBuy);
                }
                else
                {
                    ret = tradeRequest.Stock.IsSharePriceWithBufferLessThan(tradeRequest.PrimaryLimit, tradeFilterPreference.CapacityPrimaryLimitSell);
                }
            }

            decimal filteredQuantity = 0;
            string filterDescription = null;

            if (ret)
            {
                filteredQuantity = startingQuantity;
                filterDescription = tradeRequest.PrimaryLimitDescription;
            }

            var filteredAmount = filteredQuantity * tradeRequest.Stock.PriceInUsd;

            var availableQuantity = startingQuantity - filteredQuantity;

            if (availableQuantity < 0)
            {
                availableQuantity = 0;
            }

            return new FilterModel()
            {
                FilterType = "Primary Limit",
                FilteredAmount = filteredAmount,
                FilteredQuantity = filteredQuantity,
                OriginalQuantity = startingQuantity,
                AvailableQuantity = availableQuantity,
                FilterDescription = filterDescription,
            };
        }
    }
}
