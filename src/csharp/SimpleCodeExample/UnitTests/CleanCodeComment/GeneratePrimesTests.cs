using System.Collections.Generic;
using FluentAssertions;
using ProductionCode.CleanCodeComment;
using Xunit;

namespace UnitTests.CleanCodeComment
{
    public class GeneratePrimesTests
    {
        public static IEnumerable<object[]> GetData(int numTests)
        {
            yield return new object[] {1, new int[0]};
            yield return new object[] {2, new [] { 2 }};
            yield return new object[] {3, new [] { 2, 3 }};
            yield return new object[] {4, new [] { 2, 3 }};
            yield return new object[] {5, new [] { 2, 3, 5 }};
        }

        [Theory]
        [MemberData(nameof(GetData), parameters: 2)]
        public void ShouldWork(
            int maxValue,
            int[] expected)
        {
            GeneratePrimes.Generate(maxValue)
                .Should().Equal(expected);
        }
    }
}