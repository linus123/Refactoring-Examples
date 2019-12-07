using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Tests.Chapter02
{
    public class GuessTests
    {
        [Fact]
        public void ShouldReturnExpectedStringGivenZeroCount()
        {
            var messages = new List<string>();

            var guess = CreateGuess(messages);

            guess.PrintGuessStatistics("electron", 0);

            AssertSingleMessage(
                "There are no electrons",
                messages);
        }

        [Fact]
        public void ShouldReturnSingularStringGivenOneCount()
        {
            var messages = new List<string>();

            var guess = CreateGuess(messages);

            guess.PrintGuessStatistics("neutrino", 1);

            AssertSingleMessage(
                "There is 1 neutrino",
                messages);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]

        public void ShouldReturnPluralStringGivenCountTwoOrGreater(
            int itemCount)
        {
            var messages = new List<string>();

            var guess = CreateGuess(messages);

            guess.PrintGuessStatistics("quark", itemCount);

            AssertSingleMessage(
                $"There are {itemCount} quarks",
                messages);
        }

        private static Guess CreateGuess(
            List<string> messages)
        {
            return new Guess(message => messages.Add(message));
        }

        private static void AssertSingleMessage(
            string expectedMessage,
            List<string> messages)
        {
            messages.Should().HaveCount(1);
            messages[0].Should().Be(expectedMessage);
        }
    }
}