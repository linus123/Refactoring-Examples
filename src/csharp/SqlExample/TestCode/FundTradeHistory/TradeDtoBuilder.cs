using System;
using Bogus;
using ProductionCode.FundTradeHistory;

namespace TestCode.FundTradeHistory
{
    public class TradeDtoBuilder
    {
        private Faker<TradeDto> _faker;

        public TradeDtoBuilder()
        {
            _faker = new Faker<TradeDto>()
                .RuleFor(d => d.StockId, Guid.NewGuid)
                .RuleFor(d => d.Shares, f => f.Random.Decimal(-500, 500))
                .RuleFor(d => d.BrokerCode, f => f.Lorem.Word().ToUpper());
        }

        public TradeDtoBuilder WithStockId(
            Guid v)
        {
            _faker = _faker.RuleFor(d => d.StockId, v);

            return this;
        }

        public TradeDtoBuilder WithTradeDate(
            DateTime v)
        {
            _faker = _faker.RuleFor(d => d.TradeDate, v);

            return this;
        }

        public TradeDto Create()
        {
            return _faker.Generate();
        }
    }
}