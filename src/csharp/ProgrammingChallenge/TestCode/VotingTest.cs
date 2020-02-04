using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace TestCode
{
    public class VotingTest
    {
        [Fact(DisplayName = "1 voter, 1 candidate should be able to get the winner")]
        public void Test001()
        {
            var candidateNames = new string[] { "Paul" };
            var numberOfCandidate = 1;
            var votes = new int[1][];
            votes[0] = new int[] {1};

            Voting voting = new Voting();
            var result = voting.GetWinner(candidateNames[0]);
            result.Should().Be("Paul");
        }
    }

    public class Voting
    {
        public string GetWinner(string CandidateName)
        {
            return CandidateName;
        }
    }
}
