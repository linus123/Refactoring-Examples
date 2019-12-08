using System;
using System.ComponentModel;

namespace SharedKernel.Constraints
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
            AvailQuantity = originalQuantity;
            IsApplied = true;
        }

        public virtual decimal ApplyFilter(
            TradeRequest tradeRequest,
            Profile profile)
        {
            return OriginalQuantity;
        }

        public decimal FilteredQuantity { get; set; }
        public string FilterType { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal AvailQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }

        public bool IsApplied { get; set; }

        public decimal CalculateFilteredAmountAndAvailableQuantity(TradeRequest tradeRequest)
        {
            FilteredAmount = FilteredQuantity * tradeRequest.Block.PriceInUsd;

            AvailQuantity = OriginalQuantity - FilteredQuantity;

            if (AvailQuantity < 0)
            {
                AvailQuantity = 0;
            }

            return AvailQuantity;
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
