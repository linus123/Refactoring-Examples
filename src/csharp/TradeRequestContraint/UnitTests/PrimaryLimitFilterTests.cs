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
            var primaryLimitFilter = new PrimaryLimitFilter(200);

            var tradeRequest = new TradeRequest()
            {
                Stock = new Stock()
            };

            var tradeFilterPreference = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = false
            };

            primaryLimitFilter.ApplyFilter(tradeRequest, tradeFilterPreference);

            primaryLimitFilter.FilteredQuantity.Should().Be(0);
            primaryLimitFilter.AvailQuantity.Should().Be(200);
        }
    }
}