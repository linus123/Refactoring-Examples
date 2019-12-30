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
        private const decimal Precision = 0.00001m;

        [Fact(DisplayName = "GetAccumulatedDayVolume should return shares given single trade per day and trade day is a Friday.")]
        public void Test001()
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
                .WithTradeDate(tradeDate.AddDays(-2))
                .Create();

            testHelper.InsertTradeDtos(tradeDto01, tradeDto02);

            var tradeVolumes = testHelper.CreateRepository()
                .GetTradeVolumes(tradeDate, new[] { stockId });

            var target = FindStock(tradeVolumes, tradeDto01.StockId);

            target.GetAccumulatedDayVolume(1).Should()
                .BeApproximately(Math.Abs(tradeDto01.Shares), Precision);

            target.GetAccumulatedDayVolume(2).Should()
                .BeApproximately(Math.Abs(tradeDto01.Shares) + Math.Abs(tradeDto02.Shares), Precision);

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

            var day01Shares = Math.Abs(tradeDto01.Shares)+ Math.Abs(tradeDto02.Shares);

            target.GetAccumulatedDayVolume(1).Should()
                .BeApproximately(day01Shares, Precision);

            var day02Shares = day01Shares + Math.Abs(tradeDto03.Shares);

            target.GetAccumulatedDayVolume(2).Should()
                .BeApproximately(day02Shares, Precision);

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