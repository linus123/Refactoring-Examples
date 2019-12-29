using System;
using System.Collections.Generic;
using Bogus;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class TestDataCreator
    {
//        [Fact]
        [Fact(Skip = "Only run on request")]
        public void CreateTestData()
        {
            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            tradeDataTableGateway.DeleteAll();

            var currentDate = DateTime.Now;

            var tradeDtoFaker = new Faker<TradeDto>()
                .RuleFor(d => d.TradeDate, f => f.Date.Between(currentDate.AddDays(-15), currentDate.AddDays(-1)))
                .RuleFor(d => d.BrokerCode, f => f.Lorem.Word().ToUpper())
                .RuleFor(d => d.Shares, f => f.Random.Decimal(-500, 500));

            var tradeDtos = new List<TradeDto>();

            for (int stockIndex = 0; stockIndex < 10; stockIndex++)
            {
                var stockId = Guid.NewGuid();

                for (int i = 0; i < 20; i++)
                {
                    var tradeDto = tradeDtoFaker.Generate();

                    tradeDto.StockId = stockId;

                    tradeDtos.Add(tradeDto);
                }
            }

            tradeDataTableGateway.Insert(tradeDtos.ToArray());
        }
    }
}