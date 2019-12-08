namespace SharedKernel
{
    public class OrderCapacity
    {
        public string PrimLimitDescription { get; set; }

        public Block Block { get; set; }

        public bool IsSellOutQty(
            decimal value)
        {
            return false;
        }

        public decimal CalculateUnemcumberedQty()
        {
            return 0;
        }
    }
}