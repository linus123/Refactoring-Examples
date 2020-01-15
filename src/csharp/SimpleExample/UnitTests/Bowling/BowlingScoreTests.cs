using FluentAssertions;
using Xunit;

namespace TestCode.Bowling
{
    public class BowlingScoreTests
    {
        [Fact(DisplayName = "Score should be zero given 20 rolls 0.")]
        public void Test001()
        {
            var bowlingScore = new BowlingScore();
            for (int i = 0; i < 20; i++) bowlingScore.AddRoll(0);
            bowlingScore.GrandTotal.Should().Be(0);
        }

        [Fact(DisplayName = "Score should be zero given 20 rolls 0.")]
        public void Test002()
        {
            var bowlingScore = new BowlingScore();
            bowlingScore.AddRoll(1);
            for (int i = 1; i < 20; i++) bowlingScore.AddRoll(0);
            bowlingScore.GrandTotal.Should().Be(1);
        }
    }

    public class BowlingScore
    {
        public int GrandTotal { get; internal set; }

        public void AddRoll(int numberOfPins)
        {
            GrandTotal = GrandTotal+ numberOfPins;
        }
    }
}