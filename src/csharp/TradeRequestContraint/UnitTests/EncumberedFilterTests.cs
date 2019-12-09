using System.Runtime.InteropServices;
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

            var precision = 0.00001m;

            new FilterAssert(targetFilter)
                .FilteredQuantityShouldBe(tradeRequest.EncumberedQuantity)
                .FilterTypeShouldBe("Encumbered")
                .OriginalQuantityShouldBe(tradeRequest.OriginalCapacityQuantity)
                .AvailableQuantityShouldBe(tradeRequest.HoldingsQuantity - tradeRequest.EncumberedQuantity)
                .FilteredAmountQuantityShouldBe(tradeRequest.EncumberedQuantity * stock.PriceInUsd)
                .FilterDescriptionQuantityShouldBe(null)
                .IsAppliedShouldBeTrue();

            tradeRequest.AvailableCapacityQuantity.Should().BeApproximately(170m, precision);
        }

        private class FilterAssert
        {
            private const decimal Precision = 0.00001m;

            private Filter _filter;

            public FilterAssert(
                Filter filter)
            {
                _filter = filter;
            }

            public FilterAssert FilteredQuantityShouldBe(
                decimal v)
            {
                _filter.FilteredQuantity.Should().BeApproximately(v, Precision);

                return this;
            }

            public FilterAssert FilterTypeShouldBe(
                string v)
            {
                _filter.FilterType.Should().Be(v);

                return this;
            }

            public FilterAssert OriginalQuantityShouldBe(
                decimal v)
            {
                _filter.OriginalQuantity.Should().BeApproximately(v, Precision);

                return this;
            }

            public FilterAssert AvailableQuantityShouldBe(
                decimal v)
            {
                _filter.AvailQuantity.Should().BeApproximately(v, Precision);

                return this;
            }

            public FilterAssert FilteredAmountQuantityShouldBe(
                decimal v)
            {
                _filter.FilteredAmount.Should().BeApproximately(v, Precision);

                return this;
            }

            public FilterAssert FilterDescriptionQuantityShouldBe(
                string v)
            {
                _filter.FilterDescription.Should().Be(v);

                return this;
            }

            public FilterAssert IsAppliedShouldBeTrue()
            {
                _filter.IsApplied.Should().BeTrue();

                return this;
            }
        }
    }
}