using SharedKernel;

namespace UnitTests
{
    public class TradeFilterPreferenceBuilder
    {
        private readonly TradeFilterPreference _tradeFilterPreference;

        public TradeFilterPreferenceBuilder()
        {
            _tradeFilterPreference = new TradeFilterPreference();
        }

        public TradeFilterPreferenceBuilder WithHeavilyTradedFilterActive(
            decimal tradeVolume,
            int tradeDay)
        {
            _tradeFilterPreference.IsHeavilyTradeFilterActive = true;
            _tradeFilterPreference.StockHeavilyTradeVolume = tradeVolume;
            _tradeFilterPreference.StockHeavilyTradeDay = tradeDay;

            return this;
        }

        public TradeFilterPreferenceBuilder WithPrimaryFilterActive(
            decimal buyLimit,
            decimal sellLimit)
        {
            _tradeFilterPreference.IsPrimaryFilterActive = true;
            _tradeFilterPreference.CapacityPrimaryLimitBuy = buyLimit;
            _tradeFilterPreference.CapacityPrimaryLimitSell = sellLimit;

            return this;
        }

        public TradeFilterPreference Create()
        {
            return _tradeFilterPreference;
        }
    }
}