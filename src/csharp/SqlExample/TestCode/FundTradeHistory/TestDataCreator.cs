using System;
using System.Collections.Generic;
using Bogus;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class TestDataCreator
    {
        [Fact]
//        [Fact(Skip = "Only run on request")]
        public void CreateTestData()
        {
            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            tradeDataTableGateway.DeleteAll();

            var tradeDtoFaker = new Faker<TradeDto>()
                .RuleFor(d => d.TradeDate, f => f.Date.Recent(180, DateTime.Now))
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

            // **

            var stockDataTableGateway = new StockDataTableGateway(
                LocalDatabase.ConnectionString);

            stockDataTableGateway.DeleteAll();

            var stockDataDtoFaker = new Faker<StockDataDto>()
                .RuleFor(d => d.Source, 3)
                .RuleFor(d => d.DataType, 4)
                .RuleFor(d => d.BrokerCode, f => f.Lorem.Word().ToUpper())
                .RuleFor(d => d.Value, f => f.Random.Decimal(-500, 500));

            var stockDataDtos = new List<StockDataDto>();

            for (int stockIndex = 0; stockIndex < 10; stockIndex++)
            {
                var stockId = Guid.NewGuid();

                for (int i = 0; i < 20; i++)
                {
                    var stockDataDto = stockDataDtoFaker.Generate();

                    stockDataDto.StockId = stockId;
                    stockDataDto.DataDate = DateTime.Now.AddDays(i);

                    stockDataDtos.Add(stockDataDto);
                }
            }

            stockDataTableGateway.Insert(stockDataDtos.ToArray());
        }
    }
}