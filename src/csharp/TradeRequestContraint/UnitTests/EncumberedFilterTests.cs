using FluentAssertions;
using SharedKernel;
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

            var stock = new StockBuilder()
                .Create();

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithEncumberedQuantities(200, 30)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new [] {tradeRequest});

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
        }

        [Fact]
        public void ShouldNotFilterWhenTradeIsBuy()
        {
            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = true
            };

            var stock = new StockBuilder()
                .Create();

            var tradeRequest = TradeRequestBuilder.Buy()
                .WithEncumberedQuantities(200, 30)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);
        }

        [Fact]
        public void ShouldFilterWhenTradeIsSell()
        {
            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = true
            };

            var stock = new StockBuilder()
                .Create();

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithOriginalCapacityQuantity(200)
                .WithEncumberedQuantities(200, 30)
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(1);

            var targetFilter = tradeRequest.Filters[0];
            
            new FilterAssert(targetFilter)
                .FilteredQuantityShouldBe(tradeRequest.EncumberedQuantity)
                .FilterTypeShouldBe("Encumbered")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(tradeRequest.HoldingsQuantity - tradeRequest.EncumberedQuantity)
                .FilteredAmountQuantityShouldBe(tradeRequest.EncumberedQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();

            var precision = 0.00001m;

            tradeRequest.AvailableCapacityQuantity.Should().BeApproximately(170m, precision);
        }

        [Fact]
        public void ShouldNotApplyFilterWhenEncumberedIsGreaterThenOriginalQuantity()
        {
            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsCapacityEncumberedSharesFilterActive = true
            };

            var stock = new StockBuilder()
                .Create();

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithOriginalCapacityQuantity(20)
                .WithEncumberedQuantities(200, 30)
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