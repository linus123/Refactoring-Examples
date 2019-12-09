using FluentAssertions;
using SharedKernel;
using SharedKernel.Filters;
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

            var filter = new PrimaryLimitFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
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

            var filter = new PrimaryLimitFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
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

            var filter = new PrimaryLimitFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilterTypeShouldBe("Primary Limit")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
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
            
            var filter = new PrimaryLimitFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            new FilterAssert(filter)
                .FilteredQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .FilterTypeShouldBe("Primary Limit")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
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

            var filter = new PrimaryLimitFilter(tradeRequest.OriginalCapacityQuantity);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
        }
    }
}