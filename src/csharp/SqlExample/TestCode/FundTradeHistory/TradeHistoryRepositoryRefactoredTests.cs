using System;
using System.Linq;
using FluentAssertions;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class TradeHistoryRepositoryRefactoredTests
    {
        [Fact]
        public void ShouldReturnSharesGivenSingleDay()
        {
            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            var tradeDate = new DateTime(2010, 1, 10);

            var tradeDto = new TradeDto()
            {
                StockId = Guid.NewGuid(),
                TradeDate = tradeDate.AddDays(-1),
                BrokerCode = "123",
                Shares = 100m
            };

            var tradeId = tradeDataTableGateway.Insert(tradeDto);

            var tradeHistoryRepositoryRefactored = new TradeHistoryRepositoryRefactored(
                tradeDataTableGateway);

            var tradeVolumes = tradeHistoryRepositoryRefactored.GetTradeVolumes(tradeDate, new[] {tradeDto.StockId});

            tradeVolumes.Should().HaveCountGreaterThan(0);

            var target = tradeVolumes.FirstOrDefault(v => v.StockId == tradeDto.StockId);

            target.Should().NotBeNull();

            target.GetAccumulatedDayVolume(1).Should().BeApproximately(100m, 0.00001m);

            tradeDataTableGateway.DeleteById(new []{ tradeId });
        }

        [Fact]
        public void ShouldReturnSumSharesGivenTwoTradesInCurrentDayDay()
        {
            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            var tradeDate = new DateTime(2019, 12, 27);

            var stockId = Guid.NewGuid();

            var tradeDto01 = new TradeDto()
            {
                StockId = stockId,
                TradeDate = tradeDate.AddDays(-1),
                BrokerCode = "123",
                Shares = 100m
            };

            var tradeId01 = tradeDataTableGateway.Insert(tradeDto01);

            var tradeDto02 = new TradeDto()
            {
                StockId = stockId,
                TradeDate = tradeDate.AddDays(-1),
                BrokerCode = "123",
                Shares = -200m
            };

            var tradeId02 = tradeDataTableGateway.Insert(tradeDto02);

            var tradeHistoryRepositoryRefactored = new TradeHistoryRepositoryRefactored(
                tradeDataTableGateway);

            var tradeVolumes = tradeHistoryRepositoryRefactored.GetTradeVolumes(tradeDate, new[] { stockId });

            tradeVolumes.Should().HaveCountGreaterThan(0);

            var target = tradeVolumes.FirstOrDefault(v => v.StockId == stockId);

            target.Should().NotBeNull();

            target.GetAccumulatedDayVolume(1).Should().BeApproximately(300m, 0.00001m);

            tradeDataTableGateway.DeleteById(new[] { tradeId01, tradeId02 });
        }

    }
}