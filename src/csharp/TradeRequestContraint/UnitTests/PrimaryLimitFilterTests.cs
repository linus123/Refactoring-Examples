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

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                PrimaryLimit = 1000,
                Stock = new Stock()
                {
                    SharePrice = 50
                },
                TradeFilterPreference = tradeFilterPreference
            };

            var filter = new PrimaryLimitFilter(200);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);
        }

        [Fact]
        public void ShouldNotApplyFilterWhenNotFilterByBufferForBuy()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                PrimaryLimit = 1000,
                Stock = new Stock()
                {
                    SharePrice = 50
                },
                TradeFilterPreference = tradeFilterPreference
            };

            var filter = new PrimaryLimitFilter(200);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);
        }

        [Fact]
        public void ShouldApplyFilterWhenFilterByBufferForBuy()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                PrimaryLimit = 10,
                Stock = new Stock()
                {
                    SharePrice = 50
                },
                TradeFilterPreference = tradeFilterPreference
            };

            var filter = new PrimaryLimitFilter(200);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(200);
            filter.AvailableQuantity.Should().Be(0);
        }

        [Fact]
        public void ShouldApplyFilterWhenFilterByBufferForSell()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                PrimaryLimit = 1000,
                Stock = new Stock()
                {
                    SharePrice = 50
                },
                TradeFilterPreference = tradeFilterPreference
            };

            var filter = new PrimaryLimitFilter(200);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(200);
            filter.AvailableQuantity.Should().Be(0);
        }

        [Fact]
        public void ShouldNotApplyFilterWhenNotFilterByBufferForSell()
        {
            var tradeFilterPreference = new TradeFilterPreferenceBuilder()
                .WithPrimaryFilterActive(0.01m, 0.02m)
                .Create();

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                PrimaryLimit = 10,
                Stock = new Stock()
                {
                    SharePrice = 50
                },
                TradeFilterPreference = tradeFilterPreference
            };

            var filter = new PrimaryLimitFilter(200);

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);
        }
    }
}