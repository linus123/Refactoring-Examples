using System;
using System.ComponentModel;

namespace SharedKernel.Filters
{
    public class Filter
    {
        public Filter()
        {

        }

        public Filter(decimal originalQuantity)
        {
            FilterType = GetClassDescription(this.GetType());
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
            IsApplied = true;
        }

        public virtual decimal ApplyFilter(
            TradeRequest tradeRequest,
            TradeFilterPreference tradeFilterPreference)
        {
            return OriginalQuantity;
        }

        public decimal FilteredQuantity { get; set; }
        public string FilterType { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }

        public bool IsApplied { get; set; }

        public decimal CalculateFilteredAmountAndAvailableQuantity(TradeRequest tradeRequest)
        {
            FilteredAmount = FilteredQuantity * tradeRequest.Stock.PriceInUsd;

            AvailableQuantity = OriginalQuantity - FilteredQuantity;

            if (AvailableQuantity < 0)
            {
                AvailableQuantity = 0;
            }

            return AvailableQuantity;
        }

        private static string GetClassDescription(Type type)
        {
            var descriptions = (DescriptionAttribute[])
                type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }
    }
}
