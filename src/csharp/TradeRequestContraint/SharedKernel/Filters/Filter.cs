namespace SharedKernel.Filters
{
    public class Filter
    {
        public Filter(decimal originalQuantity)
        {
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
        }

        public decimal FilteredQuantity { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }
    }
}
