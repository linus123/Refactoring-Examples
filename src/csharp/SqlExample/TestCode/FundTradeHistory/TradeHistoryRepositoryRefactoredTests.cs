using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class TradeHistoryRepositoryRefactoredTests
    {
        public class TestHelper
        {
            private readonly TradeDataTableGateway _tradeDataTableGateway;
            private readonly List<int> _insertedTradeIds;

            public TestHelper()
            {
                _tradeDataTableGateway = new TradeDataTableGateway(LocalDatabase.ConnectionString);

                _insertedTradeIds = new List<int>();
            }

            public int InsertTradeDto(
                TradeDto tradeDto)
            {
                var tradeId = _tradeDataTableGateway.Insert(tradeDto);

                _insertedTradeIds.Add(tradeId);

                return tradeId;
            }

            public TradeHistoryRepositoryRefactored CreateRepository()
            {
                return new TradeHistoryRepositoryRefactored(
                    _tradeDataTableGateway);
            }

            public void TearDown()
            {
                _tradeDataTableGateway.DeleteById(_insertedTradeIds.ToArray());
            }
        }

        [Fact(DisplayName = "GetAccumulatedDayVolume should return shares given single trade on a single day and trade day is a Friday.")]
        public void Test001()
        {
            var testHelper = new TestHelper();

            var tradeDate = new DateTime(2019, 12, 27);

            var tradeDto = new TradeDtoBuilder()
                .WithTradeDate(tradeDate.AddDays(-1))
                .Create();

            testHelper.InsertTradeDto(tradeDto);

            var tradeHistoryRepositoryRefactored = testHelper.CreateRepository();

            var tradeVolumes = tradeHistoryRepositoryRefactored.GetTradeVolumes(tradeDate, new[] {tradeDto.StockId});

            tradeVolumes.Should().HaveCountGreaterThan(0);

            var target = tradeVolumes.FirstOrDefault(v => v.StockId == tradeDto.StockId);

            target.Should().NotBeNull();

            target.GetAccumulatedDayVolume(1).Should().BeApproximately(Math.Abs(tradeDto.Shares), 0.00001m);

            testHelper.TearDown();
        }

        [Fact(DisplayName = "GetAccumulatedDayVolume should return summed shares value given two trades on a single day and trade day is a Friday.")]
        public void Test002()
        {
            var testHelper = new TestHelper();

            var tradeDate = new DateTime(2019, 12, 27);

            var stockId = Guid.NewGuid();

            var tradeDto01 = new TradeDtoBuilder()
                .WithStockId(stockId)
                .WithTradeDate(tradeDate.AddDays(-1))
                .Create();

            testHelper.InsertTradeDto(tradeDto01);

            var tradeDto02 = new TradeDtoBuilder()
                .WithStockId(stockId)
                .WithTradeDate(tradeDate.AddDays(-1))
                .Create();

            testHelper.InsertTradeDto(tradeDto02);

            var tradeDto03 = new TradeDtoBuilder()
                .WithStockId(stockId)
                .WithTradeDate(tradeDate.AddDays(-2))
                .Create();

            testHelper.InsertTradeDto(tradeDto03);

            var tradeHistoryRepositoryRefactored = testHelper.CreateRepository();

            var tradeVolumes = tradeHistoryRepositoryRefactored.GetTradeVolumes(tradeDate, new[] { stockId });

            tradeVolumes.Should().HaveCountGreaterThan(0);

            var target = tradeVolumes.FirstOrDefault(v => v.StockId == stockId);

            target.Should().NotBeNull();

            var expectedVolume = Math.Abs(tradeDto01.Shares) + Math.Abs(tradeDto02.Shares);

            target.GetAccumulatedDayVolume(1).Should().BeApproximately(expectedVolume, 0.00001m);

            testHelper.TearDown();
        }
    }
}