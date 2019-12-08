namespace SharedKernel
{
    public class OrderCapacityCollection
    {
        private readonly OrderCapacity[] _orderCapacities;

        public OrderCapacityCollection(
            OrderCapacity[] orderCapacities)
        {
            _orderCapacities = orderCapacities;
        }

        public OrderCapacity[] GetOrderCapacities()
        {
            return _orderCapacities;
        }

        public void ApplyConstraints(Profile profile = null)
        {
            foreach (var oc in _orderCapacities)
            {
                oc.ApplyConstraints(profile);
            }
        }
    }
}