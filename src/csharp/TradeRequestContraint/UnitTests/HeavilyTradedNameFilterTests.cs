using FluentAssertions;
using SharedKernel;
using Xunit;

namespace UnitTests
{
    public class HeavilyTradedNameFilterTests
    {
        [Fact]
        public void ShouldDoNothingWhenFilterIsNotActive()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .Create();

            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequestBuilder()
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] {tradeRequest});

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);

            tradeRequest.Stock.ConstrainedByHeavilyTradedName.Should().BeFalse();
            tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked.Should().BeFalse();
        }

        [Fact]
        public void ShouldApplyFilterWhenTradeRequestIsHeavilyTraded()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithHeavilyTradedFilterActive(0.9m, 1)
                .Create();

            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequestBuilder()
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
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();

            tradeRequest.Stock.ConstrainedByHeavilyTradedName.Should().BeTrue();
            tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotApplyFilterWhenTradeRequestIsNotHeavilyTraded()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithHeavilyTradedFilterActive(0.9m, 1)
                .Create();

            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 100, 200, 110, 210, 150 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var tradeRequest = new TradeRequestBuilder()
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);

            tradeRequest.Stock.ConstrainedByHeavilyTradedName.Should().BeFalse();
            tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotApplyFilterShouldBeNoAppliedIsHeavilyTradedNameConstraintCheckedOnStockIsTrue()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithHeavilyTradedFilterActive(0.9m, 1)
                .Create();

            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            stock.IsHeavilyTradedNameConstraintChecked = true;

            var tradeRequest = new TradeRequestBuilder()
                .WithStock(stock)
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(0);
            tradeRequest.AvailableCapacityQuantity.Should().Be(tradeRequest.OriginalCapacityQuantity);

            tradeRequest.Stock.ConstrainedByHeavilyTradedName.Should().BeFalse();
            tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked.Should().BeTrue();
        }

        [Fact]
        public void ShouldApplyFilterShouldBeAppiedConstrainedByHeavilyTradedNameIsTrue()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithHeavilyTradedFilterActive(0.9m, 1)
                .Create();

            var stock = new StockBuilder()
                .Create();

            stock.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            stock.IsHeavilyTradedNameConstraintChecked = true;
            stock.ConstrainedByHeavilyTradedName = true;

            var tradeRequest = new TradeRequestBuilder()
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
                .FilterTypeShouldBe("Heavily Traded Name")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(0)
                .FilteredAmountQuantityShouldBe(tradeRequest.OriginalCapacityQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();

            tradeRequest.Stock.ConstrainedByHeavilyTradedName.Should().BeTrue();
            tradeRequest.Stock.IsHeavilyTradedNameConstraintChecked.Should().BeTrue();
        }
    }
}