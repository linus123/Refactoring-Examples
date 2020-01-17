using FluentAssertions;
using Xunit;

namespace TestCode.Bowling
{
    public class BowlingScoreTests
    {
        [Theory(DisplayName = "Score should be zero given 20 rolls 0.")]
        [InlineData(0, 0)]
        [InlineData(1, 20)]
        public void Test001(
            int numberOfPins,
            int expectedScore)
        {
            var bowlingScore = new BowlingScore();
            AddRolls(numberOfPins, 20, bowlingScore);
            bowlingScore.GrandTotal.Should().Be(expectedScore);
        }

        private static void AddRolls(
            int numberOfPins,
            int maxPinCount,
            BowlingScore bowlingScore)
        {
            for (int i = 0; i < maxPinCount; i++)
                bowlingScore.AddRoll(numberOfPins);
        }

        [Fact(DisplayName = "Score should be one given a single roll of 1.")]
        public void Test002()
        {
            var bowlingScore = new BowlingScore();
            bowlingScore.AddRoll(1);
            for (int i = 1; i < 20; i++) bowlingScore.AddRoll(0);
            bowlingScore.GrandTotal.Should().Be(1);
        }

        [Fact(DisplayName = "Score should be correct given a spare in first frame")]
        public void Test003()
        {
            var bowlingScore = new BowlingScore();
            bowlingScore.AddRoll(1);
            bowlingScore.AddRoll(9);
            bowlingScore.AddRoll(1);
            for (int i = 3; i < 20; i++) bowlingScore.AddRoll(0);
            bowlingScore.GrandTotal.Should().Be(12);
        }

        [Fact(DisplayName = "Score should be correct given a strike in first frame")]
        public void Test004()
        {
            var bowlingScore = new BowlingScore();
            bowlingScore.AddRoll(10);
            bowlingScore.AddRoll(4);
            bowlingScore.AddRoll(2);
            for (int i = 3; i < 19; i++) bowlingScore.AddRoll(0);
            bowlingScore.GrandTotal.Should().Be(22);
        }

        // [Theory(DisplayName = "All test")]
        // [InlineData(new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, 0)]
        // [InlineData(new int[] {1,9,1,0,0,0,0,0,0,0,0,0,0,0,0}, 12)]
        // public void Test004(
        //     int[] allRolls,
        //     int expecedScore)
        // {
        //     var bowlingScore = new BowlingScore();
        //     bowlingScore.AddRoll(1);
        //     bowlingScore.AddRoll(9);
        //     bowlingScore.AddRoll(1);
        //     for (int i = 3; i < 20; i++) bowlingScore.AddRoll(0);
        //     bowlingScore.GrandTotal.Should().Be(12);
        // }
    }

    public class BowlingScore
    {
        public int GrandTotal { get; internal set; }

        private int[] _rolls;
        private int _currentRoleIndex;

        public BowlingScore()
        {
            _rolls = new int[20];
            _currentRoleIndex = 0;
        }

        public void AddRoll(int numberOfPins)
        {
            _rolls[_currentRoleIndex] = numberOfPins;
            GrandTotal = GrandTotal + numberOfPins;
            if (_currentRoleIndex > 1 && _currentRoleIndex % 2 == 0)
            {
                if (_rolls[_currentRoleIndex - 1] + _rolls[_currentRoleIndex - 2] == 10)
                {
                    GrandTotal += numberOfPins;
                }
            }

            _currentRoleIndex += 1;
        }
    }

}