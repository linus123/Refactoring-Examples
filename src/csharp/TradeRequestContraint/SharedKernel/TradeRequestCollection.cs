namespace SharedKernel
{
    public class TradeRequestCollection
    {
        private readonly TradeRequest[] _tradeRequests;

        public TradeRequestCollection(
            TradeRequest[] tradeRequests)
        {
            _tradeRequests = tradeRequests;
        }

        public void ApplyFilters(TradeFilterPreference tradeFilterPreference = null)
        {
            foreach (var oc in _tradeRequests)
            {
                if (tradeFilterPreference == null)
                {
                    oc.ApplyFilters(oc.TradeFilterPreference);
                }
                else
                {
                    oc.ApplyFilters(tradeFilterPreference);
                }

            }
        }
    }
}