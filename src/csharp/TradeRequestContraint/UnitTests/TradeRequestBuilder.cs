using Bogus;
using SharedKernel;

namespace UnitTests
{
    public class TradeRequestBuilder
    {
        private Faker<TradeRequest> _faker;
        private Stock _stock;
        private TradeFilterPreference _tradeFilterPreference;

        public static TradeRequestBuilder Sell()
        {
            return new TradeRequestBuilder()
                .WithSideAsSell();
        }

        public static TradeRequestBuilder Buy()
        {
            return new TradeRequestBuilder()
                .WithSideAsBuy();
        }

        public TradeRequestBuilder()
        {
            _faker = new Faker<TradeRequest>()
                .RuleFor(m => m.OriginalCapacityQuantity, f => f.Random.Decimal(1, 6000));
        }

        public TradeRequestBuilder WithSideAsSell()
        {
            _faker = _faker.RuleFor(m => m.TradeSide, TradeSide.Sell);

            return this;
        }

        public TradeRequestBuilder WithSideAsBuy()
        {
            _faker = _faker.RuleFor(m => m.TradeSide, TradeSide.Buy);

            return this;
        }

        public TradeRequestBuilder WithEncumberedQuantities(
            decimal holdingQuantity,
            decimal encumberedQuantity)
        {
            _faker = _faker
                .RuleFor(m => m.HoldingsQuantity, holdingQuantity)
                .RuleFor(m => m.EncumberedQuantity, encumberedQuantity);

            return this;
        }

        public TradeRequestBuilder WithOriginalCapacityQuantity(
            decimal v)
        {
            _faker = _faker.RuleFor(m => m.OriginalCapacityQuantity, v);

            return this;
        }

        public TradeRequestBuilder WithStock(
            Stock stock)
        {
            _stock = stock;

            return this;
        }

        public TradeRequestBuilder WithTradeFilterPreference(
            TradeFilterPreference tradeFilterPreference)
        {
            _tradeFilterPreference = tradeFilterPreference;

            return this;
        }

        public TradeRequest Create()
        {
            var tradeRequest = _faker.Generate();

            if (_stock != null)
                tradeRequest.Stock = _stock;

            if (_tradeFilterPreference != null)
                tradeRequest.TradeFilterPreference = _tradeFilterPreference;

            return tradeRequest;
        }
    }
}