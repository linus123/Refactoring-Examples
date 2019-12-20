using FluentAssertions;
using ProductionCode.CleanCodeComment;
using Xunit;

namespace UnitTests.CleanCodeComment
{
    public class GeneratePrimesTests
    {
        [Fact]
        public void ShouldWork()
        {
            var generate = GeneratePrimes.Generate(20);

            generate.Length.Should().Be(8);

            generate.Should().Equal(2, 3, 5, 7, 11, 13, 17, 19);
        }
    }
}