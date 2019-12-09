using System;
using Bogus;
using SharedKernel;

namespace UnitTests
{
    public class StockBuilder
    {
        private Faker<Stock> _faker;

        public StockBuilder()
        {
            _faker = new Faker<Stock>()
                .RuleFor(m => m.StockId, f => Guid.NewGuid().ToString())
                .RuleFor(m => m.PriceInUsd, f => f.Random.Decimal(1, 500));
        }

        public Stock Create()
        {
            return _faker.Generate();
        }
    }

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

        public TradeFilterPreference Create()
        {
            return _tradeFilterPreference;
        }
    }
}