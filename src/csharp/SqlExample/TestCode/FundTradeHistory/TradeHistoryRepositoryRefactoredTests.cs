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
        [Fact(DisplayName = "GetAccumulatedDayVolume should return shares given single trade on a single day and trade day is a Friday.")]
        public void Test001()
        {
            var testHelper = new TestHelper();

            var tradeDate = new DateTime(2019, 12, 27);

            var tradeDto = new TradeDtoBuilder()
                .WithTradeDate(tradeDate.AddDays(-1))
                .Create();

            testHelper.InsertTradeDtos(tradeDto);

            var tradeVolumes = testHelper.CreateRepository()
                .GetTradeVolumes(tradeDate, new[] {tradeDto.StockId});

            var target = FindStock(tradeVolumes, tradeDto.StockId);

            target.GetAccumulatedDayVolume(1).Should()
                .BeApproximately(Math.Abs(tradeDto.Shares), 0.00001m);

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

            var tradeDto02 = new TradeDtoBuilder()
                .WithStockId(stockId)
                .WithTradeDate(tradeDate.AddDays(-1))
                .Create();

            var tradeDto03 = new TradeDtoBuilder()
                .WithStockId(stockId)
                .WithTradeDate(tradeDate.AddDays(-2))
                .Create();

            testHelper.InsertTradeDtos(
                tradeDto01,
                tradeDto02,
                tradeDto03);

            var tradeVolumes = testHelper.CreateRepository()
                .GetTradeVolumes(tradeDate, new[] { stockId });

            var target = FindStock(tradeVolumes, stockId);

            var expectedVolume = Math.Abs(tradeDto01.Shares) + Math.Abs(tradeDto02.Shares);

            target.GetAccumulatedDayVolume(1).Should()
                .BeApproximately(expectedVolume, 0.00001m);

            testHelper.TearDown();
        }

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

            public void InsertTradeDtos(
                params TradeDto[] tradeDto)
            {
                foreach (var dto in tradeDto)
                {
                    InsertTradeDto(dto);
                }
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

        private static TradeHistoryRepositoryRefactored.Stock FindStock(
            TradeHistoryRepositoryRefactored.Stock[] tradeVolumes,
            Guid stockId)
        {
            var target = tradeVolumes.FirstOrDefault(v => v.StockId == stockId);
            target.Should().NotBeNull();
            return target;
        }
    }
}