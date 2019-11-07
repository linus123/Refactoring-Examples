﻿using FluentAssertions;
using Xunit;

namespace VideoStore
{
    public class VideoStoreTests
    {
        [Fact]
        public void TestSingleNewReleaseStatement()
        {
            var customer = new CustomerBuilder()
                .Create();

            var newRelease = new MovieBuilder()
                .CreateNewRelease();

            customer.AddRental(new Rental(newRelease, 3));

            var statement = customer.GetStatement();

            AssertAmountAndPoints(statement, 9.0, 2);
        }

        [Fact]
        public void TestDualNewReleaseStatement()
        {
            var customer = new CustomerBuilder()
                .Create();

            var newRelease1 = new MovieBuilder()
                .CreateNewRelease();

            var newRelease2 = new MovieBuilder()
                .CreateNewRelease();

            customer.AddRental(new Rental(newRelease1, 3));
            customer.AddRental(new Rental(newRelease2, 3));

            var statement = customer.GetStatement();

            AssertAmountAndPoints(statement, 18.0, 4);
        }

        [Fact]
        public void TestSingleChildrensStatement()
        {
            var customer = new CustomerBuilder()
                .Create();

            var childrens = new MovieBuilder()
                .CreateChildrens();

            customer.AddRental(new Rental(childrens, 3));

            var statement = customer.GetStatement();

            AssertAmountAndPoints(statement, 1.5, 1);
        }

        [Fact]
        public void TestMultipleRegularStatement()
        {
            var customer = new CustomerBuilder()
                .Create();

            var regular1 = new MovieBuilder()
                .CreateRegular();

            var regular2 = new MovieBuilder()
                .CreateRegular();

            var regular3 = new MovieBuilder()
                .CreateRegular();

            customer.AddRental(new Rental(regular1, 1));
            customer.AddRental(new Rental(regular2, 2));
            customer.AddRental(new Rental(regular3, 3));

            var statement = customer.GetStatement();

            AssertAmountAndPoints(statement, 7.5, 3);
        }

        [Fact]
        public void TestRentalStatementFormat()
        {
            var customer = new CustomerBuilder()
                .WithName("Customer Name")
                .Create();

            var regular1 = new MovieBuilder()
                .WithTitle("Regular 1")
                .CreateRegular();

            var regular2 = new MovieBuilder()
                .WithTitle("Regular 2")
                .CreateRegular();

            var regular3 = new MovieBuilder()
                .WithTitle("Regular 3")
                .CreateRegular();

            customer.AddRental(new Rental(regular1, 1));
            customer.AddRental(new Rental(regular2, 2));
            customer.AddRental(new Rental(regular3, 3));

            const string expectedStatement = "Rental Record for Customer Name\n" +
                                             "\tRegular 1\t2.0\n" +
                                             "\tRegular 2\t2.0\n" +
                                             "\tRegular 3\t3.5\n" +
                                             "Amount owed is 7.5\n" +
                                             "You earned 3 frequent renter points";

            var statement = customer.GetStatement();

            statement.Should().Be(expectedStatement);
        }

        private void AssertAmountAndPoints(
            string statement,
            double expectedAmount,
            int expectedPoints)
        {
            statement
                .Should()
                .Contain($"Amount owed is {expectedAmount:F1}");

            statement
                .Should()
                .Contain($"You earned {expectedPoints} frequent renter points");
        }
    }
}