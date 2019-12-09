namespace SharedKernel.Filters
{
    public class Filter
    {
        public Filter(decimal originalQuantity)
        {
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
            IsApplied = true;
        }

        public decimal FilteredQuantity { get; set; }
        public string FilterType { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }

        public bool IsApplied { get; set; }
    }
}
