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
            foreach (var req in _tradeRequests)
            {
                if (tradeFilterPreference == null)
                {
                    req.ApplyFilters(req.TradeFilterPreference);
                }
                else
                {
                    req.ApplyFilters(tradeFilterPreference);
                }
            }
        }
    }
}