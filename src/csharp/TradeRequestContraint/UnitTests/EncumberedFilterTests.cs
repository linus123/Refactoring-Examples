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
            var encumberedFilter = new EncumberedFilter(100);

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

            encumberedFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            encumberedFilter.FilteredQuantity.Should().Be(0);
            encumberedFilter.AvailQuantity.Should().Be(100);
        }

        [Fact]
        public void ShouldNotFilterWhenTradeIsBuy()
        {
            var encumberedFilter = new EncumberedFilter(100);

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

            encumberedFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            encumberedFilter.FilteredQuantity.Should().Be(0);
            encumberedFilter.AvailQuantity.Should().Be(100);
        }

        [Fact]
        public void ShouldFilterWhenTradeIsSell()
        {
            var encumberedFilter = new EncumberedFilter(200);

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

            encumberedFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            encumberedFilter.FilteredQuantity.Should().Be(30);
            encumberedFilter.AvailQuantity.Should().Be(170);
        }
    }
}