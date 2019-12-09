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
}