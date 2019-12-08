namespace SharedKernel
{
    public class OrderCapacityCollection
    {
        private OrderCapacity[] _orderCapacities;

        public OrderCapacityCollection(
            OrderCapacity[] orderCapacities)
        {
            _orderCapacities = orderCapacities;
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