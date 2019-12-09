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

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithEncumberedQuantities(200, 30)
                .WithStock(new Stock()
                {
                    StockId = "0000"
                })
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

            var tradeRequest = TradeRequestBuilder.Buy()
                .WithEncumberedQuantities(200, 30)
                .WithStock(new Stock()
                {
                    StockId = "0000"
                })
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

            var tradeRequest = TradeRequestBuilder.Sell()
                .WithOriginalCapacityQuantity(200)
                .WithEncumberedQuantities(200, 30)
                .WithStock(new Stock()
                {
                    StockId = "0000"
                })
                .WithTradeFilterPreference(tradeFilterPreference)
                .Create();

            var tradeRequestCollection = new TradeRequestCollection(new[] { tradeRequest });

            tradeRequestCollection.ApplyFilters();

            tradeRequest.Filters.Should().HaveCount(1);

            var targetFilter = tradeRequest.Filters[0];

            targetFilter.FilteredQuantity.Should().Be(30m);
            targetFilter.FilterType.Should().Be("Encumbered");
            targetFilter.OriginalQuantity.Should().Be(200);
            targetFilter.AvailQuantity.Should().Be(170m);
            targetFilter.FilteredAmount.Should().Be(0);
            targetFilter.FilterDescription.Should().BeNull();
            targetFilter.IsApplied.Should().BeTrue();

            tradeRequest.AvailableCapacityQuantity.Should().Be(170m);
        }
    }
}