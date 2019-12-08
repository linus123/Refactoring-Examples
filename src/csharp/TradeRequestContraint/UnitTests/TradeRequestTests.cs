using FluentAssertions;
using SharedKernel;
using Xunit;

namespace UnitTests
{
    public class TradeRequestTests
    {
        [Fact]
        public void GetOriginalCapacityAmountShouldReturnExpectedValue()
        {
            var tradeRequest = new TradeRequest()
            {
                OriginalCapacityQuantity = 100,
                Stock = new Stock()
                {
                    PriceInUsd = 100
                }
            };

            tradeRequest.GetOriginalCapacityAmount().Should().Be(10_000m);
        }
    }
}