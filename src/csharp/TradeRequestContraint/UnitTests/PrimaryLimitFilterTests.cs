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
            var filter = new PrimaryLimitFilter(200);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                PrimaryLimit = 1000,
                Stock = new Stock()
                {
                    SharePrice = 50
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = false
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);
        }

        [Fact]
        public void ShouldNotApplyFilterWhenNotFilterByBufferForBuy()
        {
            var filter = new PrimaryLimitFilter(200);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                PrimaryLimit = 1000,
                Stock = new Stock()
                {
                    SharePrice = 50
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = true,
                CapacityPrimaryLimitBuy = 0.01m,
                CapacityPrimaryLimitSell = 0.02m
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);
        }

        [Fact]
        public void ShouldApplyFilterWhenFilterByBufferForBuy()
        {
            var filter = new PrimaryLimitFilter(200);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Buy,
                PrimaryLimit = 10,
                Stock = new Stock()
                {
                    SharePrice = 50
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = true,
                CapacityPrimaryLimitBuy = 0.01m,
                CapacityPrimaryLimitSell = 0.02m
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(200);
            filter.AvailableQuantity.Should().Be(0);
        }

        [Fact]
        public void ShouldApplyFilterWhenFilterByBufferForSell()
        {
            var filter = new PrimaryLimitFilter(200);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                PrimaryLimit = 1000,
                Stock = new Stock()
                {
                    SharePrice = 50
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = true,
                CapacityPrimaryLimitBuy = 0.01m,
                CapacityPrimaryLimitSell = 0.02m
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(200);
            filter.AvailableQuantity.Should().Be(0);
        }

        [Fact]
        public void ShouldNotApplyFilterWhenNotFilterByBufferForSell()
        {
            var filter = new PrimaryLimitFilter(200);

            var tradeRequest = new TradeRequest()
            {
                TradeSide = TradeSide.Sell,
                PrimaryLimit = 10,
                Stock = new Stock()
                {
                    SharePrice = 50
                }
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = true,
                CapacityPrimaryLimitBuy = 0.01m,
                CapacityPrimaryLimitSell = 0.02m
            };

            filter.ApplyFilter(tradeRequest, tradeFilterPreference);

            filter.FilteredQuantity.Should().Be(0);
            filter.AvailableQuantity.Should().Be(200);
        }
    }
}