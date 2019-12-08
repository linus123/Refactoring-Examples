using FluentAssertions;
using SharedKernel;
using SharedKernel.Filters;
using Xunit;

namespace UnitTests
{
    public class HeavilyTradedNameFilterTests
    {
        [Fact]
        public void ShouldDoNothingWhenFilterIsNotActive()
        {
            var heavilyTradedNameFilter = new HeavilyTradedNameFilter(200);

            var stock = new Stock()
            {
                StockId = "1234"
            };

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequest()
            {
                Stock = stock
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsHeavilyTradeFilterActive = false,
                StockHeavilyTradeVolume = 0.90m,
                StockHeavilyTradeDay = 1
            };

            heavilyTradedNameFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            heavilyTradedNameFilter.FilteredQuantity.Should().Be(0);
            heavilyTradedNameFilter.AvailQuantity.Should().Be(200);
        }

        [Fact]
        public void ShouldApplyFilterWhenTradeRequestIsHeavilyTraded()
        {
            var heavilyTradedNameFilter = new HeavilyTradedNameFilter(200);

            var stock = new Stock()
            {
                StockId = "1234"
            };

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequest()
            {
                Stock = stock
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsHeavilyTradeFilterActive = true,
                StockHeavilyTradeVolume = 0.90m,
                StockHeavilyTradeDay = 1
            };

            heavilyTradedNameFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            heavilyTradedNameFilter.FilteredQuantity.Should().Be(200);
            heavilyTradedNameFilter.AvailQuantity.Should().Be(0);

        }

        [Fact]
        public void ShouldNotApplyFilterWhenTradeRequestIsNotHeavilyTraded()
        {
            var heavilyTradedNameFilter = new HeavilyTradedNameFilter(200);

            var stock = new Stock()
            {
                StockId = "1234"
            };

            stock.SetVolumes(
                new decimal[] { 100, 200, 110, 210, 150 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequest()
            {
                Stock = stock
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsHeavilyTradeFilterActive = true,
                StockHeavilyTradeVolume = 0.90m,
                StockHeavilyTradeDay = 1
            };

            heavilyTradedNameFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            heavilyTradedNameFilter.FilteredQuantity.Should().Be(0);
            heavilyTradedNameFilter.AvailQuantity.Should().Be(200);
        }
    }
}