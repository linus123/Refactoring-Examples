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

        public void ApplyConstraints(TradeFilterPreference tradeFilterPreference = null)
        {
            foreach (var oc in _orderCapacities)
            {
                if (tradeFilterPreference == null)
                {
                    oc.ApplyConstraints(oc.TradeFilterPreference);
                }
                else
                {
                    oc.ApplyConstraints(tradeFilterPreference);
                }

            }
        }
    }
}