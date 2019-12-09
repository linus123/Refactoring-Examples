using FluentAssertions;
using SharedKernel;
using Xunit;

namespace UnitTests
{
    public class PrimaryLimitFilterTests
    {
        [Fact]
        public void ShouldNotApplyFilterWhenIsPrimaryFilterActiveIsFalse()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .Create();

            var stock = new StockBuilder()
                .WithSharePrice(50)
                .Create();

            var tradeRequest = TradeRequestBuilder.Buy()
                .WithPrimaryLimit(1000)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
        }

        [Fact]
        public void ShouldNotApplyFilterWhenNotFilterByBufferForBuy()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var stock = new StockBuilder()
                .WithSharePrice(50)
                .Create();

            var tradeRequest = TradeRequestBuilder.Buy()
                .WithPrimaryLimit(1000)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
        }

        [Fact]
        public void ShouldApplyFilterWhenFilterByBufferForBuy()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var stock = new StockBuilder()
                .WithSharePrice(50)
                .Create();

            var tradeRequest = TradeRequestBuilder.Buy()
                .WithPrimaryLimit(10)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(1);
            tradeRequest.AvailableCapacityQuantity.Should().Be(0);

            var filter = tradeRequest.Filters[0];

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilterTypeShouldBe("Primary Limit")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(tradeRequest.PrimaryLimitDescription)
                .IsAppliedShouldBeTrue();
        }

        [Fact]
        public void ShouldApplyFilterWhenFilterByBufferForSell()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var stock = new StockBuilder()
                .WithSharePrice(50)
                .Create();

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithPrimaryLimit(1000)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(1);
            tradeRequest.AvailableCapacityQuantity.Should().Be(0);

            var filter = tradeRequest.Filters[0];

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilterTypeShouldBe("Primary Limit")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(tradeRequest.PrimaryLimitDescription)
                .IsAppliedShouldBeTrue();
        }

        [Fact]
        public void ShouldNotApplyFilterWhenNotFilterByBufferForSell()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var stock = new StockBuilder()
                .WithSharePrice(50)
                .Create();

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithPrimaryLimit(10)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
        }
    }
}