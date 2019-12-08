using FluentAssertions;
using SharedKernel;
using SharedKernel.Filters;
using Xunit;

namespace UnitTests
{
    public class EncumberedFilterTests
    {
        [Fact]
        public void ShouldNotFilterWhenFilterIsNotActive()
        {
            var filter = new EncumberedFilter(100);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                HoldingsQuantity = 200,
                EncumberedQuantity = 30,
                Stock = new Stock()
                {
                    StockId = "0000"
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = false
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailQuantity.Should().Be(100);
        }

        [Fact]
        public void ShouldNotFilterWhenTradeIsBuy()
        {
            var filter = new EncumberedFilter(100);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                HoldingsQuantity = 200,
                EncumberedQuantity = 30,
                Stock = new Stock()
                {
                    StockId = "0000"
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = true
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailQuantity.Should().Be(100);
        }

        [Fact]
        public void ShouldFilterWhenTradeIsSell()
        {
            var filter = new EncumberedFilter(200);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                HoldingsQuantity = 200,
                EncumberedQuantity = 30,
                Stock = new Stock()
                {
                    StockId = "0000"
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = true
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(30);
            filter.AvailQuantity.Should().Be(170);
        }
    }
}