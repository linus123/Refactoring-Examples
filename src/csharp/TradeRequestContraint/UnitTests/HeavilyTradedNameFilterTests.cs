using SharedKernel.Filters;
using Xunit;

namespace UnitTests
{
    public class HeavilyTradedNameFilterTests
    {
        [Fact]
        public void ShouldDoNothingWhenFilterIsNotActive()
        {
            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequestBuilder()
                .WithStock(stock)
                .Create();

            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .Create();

            var filter = new HeavilyTradedNameFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(0)
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilteredAmountQuantityShouldBe(0)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();
        }

        [Fact]
        public void ShouldApplyFilterWhenTradeRequestIsHeavilyTraded()
        {
            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequestBuilder()
                .WithStock(stock)
                .Create();

            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithHeavilyTradedFilterActive(0.9m, 1)
                .Create();

            var filter = new HeavilyTradedNameFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();
        }

        [Fact]
        public void ShouldNotApplyFilterWhenTradeRequestIsNotHeavilyTraded()
        {
            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 100, 200, 110, 210, 150 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequestBuilder()
                .WithStock(stock)
                .Create();

//            var tradeRequest = new TradeRequest()
//            {
//                Stock = stock
//            };

            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithHeavilyTradedFilterActive(0.9m, 1)
                .Create();

            var filter = new HeavilyTradedNameFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(0)
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilteredAmountQuantityShouldBe(0)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();
        }
    }
}