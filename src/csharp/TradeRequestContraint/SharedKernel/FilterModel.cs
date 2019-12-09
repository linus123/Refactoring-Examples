namespace SharedKernel
{
    public class FilterModel
    {
        public decimal FilteredQuantity { get; set; }
        public string FilterType { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal FilteredAmount { get; set; }
        public string FilterDescription { get; set; }
    }
}