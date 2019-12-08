namespace SharedKernel
{
    public class TradeRequestCollection
    {
        private readonly TradeRequest[] _orderCapacities;

        public TradeRequestCollection(
            TradeRequest[] orderCapacities)
        {
            _orderCapacities = orderCapacities;
        }

        public TradeRequest[] GetOrderCapacities()
        {
            return _orderCapacities;
        }

        public void ApplyConstraints(Profile profile = null)
        {
            foreach (var oc in _orderCapacities)
            {
                if (profile == null)
                {
                    oc.ApplyConstraints(oc.Profile);
                }
                else
                {
                    oc.ApplyConstraints(profile);
                }

            }
        }
    }
}