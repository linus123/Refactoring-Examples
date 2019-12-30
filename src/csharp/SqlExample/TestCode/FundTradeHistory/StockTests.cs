using System;
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

            var stock = new Stock(
                Guid.NewGuid(),
                fridayTradeDate,
                new TradeDto[0]);

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

            var tradeDtos = new TradeDto[]
            {
                tradeDto
            };

            var stock = new Stock(
                stockId,
                fridayTradeDate,
                tradeDtos);

            for (int dayCounter = 1; dayCounter <= 10; dayCounter++)
            {
                stock.GetAccumulatedDayVolume(1).Should()
                    .BeApproximately(tradeDto.GetAbsoluteValueShares(), Precision);
            }
        }

    }
}