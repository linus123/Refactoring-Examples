using System;
using System.Collections.Generic;
using FluentAssertions;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class StockTests
    {
        private const decimal Precision = 0.000001m;

        [Fact(DisplayName = "All volumes should be zero given no trades.")]
        public void Test001()
        {
            var fridayTradeDate = new DateTime(2019, 12, 27);

            var stock = new StockBuilder()
                .WithTradeDate(fridayTradeDate)
                .Create();

            for (int dayCounter = 1; dayCounter <= 10; dayCounter++)
            {
                stock.GetAccumulatedDayVolume(dayCounter)
                    .Should().BeApproximately(0m, Precision);
            }
        }

        [Fact(DisplayName = "All volumes should return value given single trade on first day.")]
        public void Test002()
        {
            var stockId = Guid.NewGuid();
            var fridayTradeDate = new DateTime(2019, 12, 27);

            var tradeDto = new TradeDtoBuilder()
                .WithStockId(stockId)
                .WithTradeDate(fridayTradeDate.AddDays(-1))
                .Create();

            var stock = new StockBuilder(stockId)
                .WithTradeDate(fridayTradeDate)
                .WithTrade(tradeDto)
                .Create();

            for (int dayCounter = 1; dayCounter <= 10; dayCounter++)
            {
                stock.GetAccumulatedDayVolume(dayCounter).Should()
                    .BeApproximately(tradeDto.GetAbsoluteValueShares(), Precision);
            }
        }

        public class StockBuilder
        {
            private Guid _stockId;
            private DateTime _tradeDate;
            private List<TradeDto> _tradeDtos;

            public StockBuilder()
            {
                _stockId = Guid.NewGuid();

                _tradeDtos = new List<TradeDto>();
            }

            public StockBuilder(
                Guid stockId) : this()
            {
                _stockId = stockId;
            }
            
            public StockBuilder WithTradeDate(
                DateTime tradeDate)
            {
                _tradeDate = tradeDate;

                return this;
            }

            public StockBuilder WithTrade(
                TradeDto tradeDto)
            {
                _tradeDtos.Add(tradeDto);

                return this;
            }

            public Stock Create()
            {
                return new Stock(
                    _stockId,
                    _tradeDate,
                    _tradeDtos.ToArray());
            }
        }

    }
}