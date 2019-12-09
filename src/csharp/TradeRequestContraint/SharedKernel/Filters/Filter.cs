namespace SharedKernel.Filters
{
    public class Filter
    {
        public Filter(decimal originalQuantity)
        {
            OriginalQuantity = originalQuantity;
            AvailableQuantity = originalQuantity;
        }

        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
    }
}
