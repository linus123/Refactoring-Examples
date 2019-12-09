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
            var filter = new HeavilyTradedNameFilter(200);

            var stock = new StockBuilder()
                .Create();

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

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(0)
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(200)
                .AvailableQuantityShouldBe(200)
                .FilteredAmountQuantityShouldBe(0)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();
        }

        [Fact]
        public void ShouldApplyFilterWhenTradeRequestIsHeavilyTraded()
        {
            var filter = new HeavilyTradedNameFilter(200);

            var stock = new StockBuilder()
                .Create();

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

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(200)
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(200)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(200 * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();
        }

        [Fact]
        public void ShouldNotApplyFilterWhenTradeRequestIsNotHeavilyTraded()
        {
            var filter = new HeavilyTradedNameFilter(200);

            var stock = new StockBuilder()
                .Create();

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

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(0)
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(200)
                .AvailableQuantityShouldBe(200)
                .FilteredAmountQuantityShouldBe(0)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();
        }
    }
}