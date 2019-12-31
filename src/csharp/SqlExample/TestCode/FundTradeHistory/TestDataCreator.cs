using System;
using System.Collections.Generic;
using System.Linq;
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
            ResetTradeData(DateTime.Now);
        }

        [Fact]
        public void TradeVolumeCompareTest()
        {
            var tradeDate = new DateTime(2019, 12, 27);

            ResetTradeData(tradeDate);

            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            var stockIds = tradeDataTableGateway.GetStockIds();

            var tradeHistoryRepository = new TradeHistoryRepository(
                LocalDatabase.ConnectionString);

            var tradeVolumeHistories = tradeHistoryRepository.GetTradeVolumes(tradeDate, stockIds);

            var tradeHistoryRepositoryRefactored = new TradeHistoryRepositoryRefactored(
                tradeDataTableGateway);

            var dataUnderTest = tradeHistoryRepositoryRefactored.GetTradeVolumes(tradeDate, stockIds);

            dataUnderTest.Should().HaveCount(tradeVolumeHistories.Length);

            foreach (var tradeVolumeHistory in tradeVolumeHistories)
            {
                tradeVolumeHistory.Accumulate10DayVolume();

                var target = dataUnderTest.FirstOrDefault(d => d.StockId == tradeVolumeHistory.StockId);

                target.Should().NotBeNull();

                var precision = 0.000001m;

                for (int dayCounter = 1; dayCounter <= 10; dayCounter++)
                {
                    target.GetAccumulatedDayVolume(dayCounter).Should().BeApproximately(
                        tradeVolumeHistory.GetAccumulatedDayVolume(dayCounter), precision,
                        $"Day '{dayCounter}' value did not match on stock '{tradeVolumeHistory.StockId}'.");
                }
            }
        }

        private static void ResetTradeData(DateTime currentDate)
        {
            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            tradeDataTableGateway.DeleteAll();

            var tradeDtoFaker = new Faker<TradeDto>()
                .RuleFor(d => d.TradeDate, f => f.Date.Between(currentDate.AddDays(-15), currentDate.AddDays(-1)).Date)
                .RuleFor(d => d.BrokerCode, f => f.Lorem.Word().ToUpper())
                .RuleFor(d => d.Shares, f => f.Random.Decimal(-500, 500));

            var tradeDtos = new List<TradeDto>();

            for (int stockIndex = 0; stockIndex < 10; stockIndex++)
            {
                var stockId = Guid.NewGuid();

                for (int i = 0; i < 20; i++)
                {
                    var tradeDto = tradeDtoFaker.Generate();

                    while (tradeDto.TradeDate.DayOfWeek == DayOfWeek.Saturday
                           || tradeDto.TradeDate.DayOfWeek == DayOfWeek.Sunday)
                        tradeDto = tradeDtoFaker.Generate();

                    tradeDto.StockId = stockId;

                    tradeDtos.Add(tradeDto);
                }
            }

            tradeDataTableGateway.Insert(tradeDtos.ToArray());
        }
    }
}