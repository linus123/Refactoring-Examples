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
            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = false
            };

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                HoldingsQuantity = 200,
                EncumberedQuantity = 30,
                OriginalCapacityQuantity = 100,
                Stock = new Stock()
                {
                    StockId = "0000"
                },
                TradeFilterPreference = tradeFilterPreference
            };

            var tradeRequestCollection = new TradeRequestCollection(new TradeRequest[1] {tradeRequest});

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(100);
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