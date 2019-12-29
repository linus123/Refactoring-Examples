using System;
using System.Collections.Generic;
using Bogus;
using FluentAssertions;
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
            ResetTradeData();
        }

        [Fact]
        public void TradeVolumeCompareTest()
        {
            ResetTradeData();

            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            var stockIds = tradeDataTableGateway.GetStockIds();

            var tradeHistoryRepository = new TradeHistoryRepository(
                LocalDatabase.ConnectionString);

            var tradeDate = DateTime.Now;

            var tradeVolumeHistories = tradeHistoryRepository.GetTradeVolumes(tradeDate, stockIds);

            var dataUnderTest = new object[0];

            dataUnderTest.Should().HaveCount(tradeVolumeHistories.Length);

        }

        private static void ResetTradeData()
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