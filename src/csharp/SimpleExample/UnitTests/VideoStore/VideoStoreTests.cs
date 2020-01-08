﻿using FluentAssertions;
using IoC;
using ProductionCode.VideoStore;
using Xunit;

namespace TestCode.VideoStore
{
    public class VideoStoreTests
    {
        private readonly Customer _customer;

        public VideoStoreTests()
        {
            _customer = new Customer("Fred");
        }

        
            NewReleaseMovie creatNewReleaseMovie()
            {
                return new NewReleaseMovie("SomeMovie");
            }
           ChildrensMovie CreateChildrensMovie()
            {
                return new ChildrensMovie("SomeMovie");
            }
            RegularMovie creaRegularMovie()
            {
                return new RegularMovie("SomeMovie");
            }
        

        [Theory]
        [InlineData(1, 3, 1)]
        [InlineData(3, 9, 2)]
        public void SingleNewReleaseStatement(
            int daysRented, 
            decimal expectedTotalAmount,
            int expectedFrequentRenterPoints)
        {
            var movie = creatNewReleaseMovie();
            _customer.AddRental(new Rental(movie, daysRented));
            _customer.Statement();
            _customer.TotalAmount.Should().BeApproximately(expectedTotalAmount, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(expectedFrequentRenterPoints);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(3, 3.5, 1)]
        public void SingleRegularStatement(int daysRented,
            decimal expectedTotalAmount,
            int expectedFrequentRenterPoints)
        {
            var movie = creaRegularMovie();
            _customer.AddRental(new Rental(movie, daysRented));
            _customer.Statement();
            _customer.TotalAmount.Should().BeApproximately(expectedTotalAmount, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(expectedFrequentRenterPoints);
        }

        [Fact]
        public void DualNewReleaseStatement()
        {
            var movie1 = creatNewReleaseMovie();
            _customer.AddRental(new Rental(movie1, 3));
            var movie2 = creatNewReleaseMovie();
            _customer.AddRental(new Rental(movie2, 3));
            _customer.Statement();
            _customer.TotalAmount.Should().BeApproximately( 18.0m, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(4);
        }

        [Theory(DisplayName = "SingleChildrens Should Create Correct TotalAmount")]
        [InlineData(1, 1.5, 1)]
        [InlineData(3, 1.5, 1)]
        [InlineData(4, 3, 1)]
        public void Test2(
            int daysRented, 
            decimal totalAmount,
            int frequentRenterPoints)
        {
            var movie = CreateChildrensMovie();
            _customer.AddRental(new Rental(movie, daysRented));
            _customer.Statement();
            _customer.TotalAmount.Should().BeApproximately(totalAmount, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(frequentRenterPoints);
        }

        [Fact]
        public void MultipleRegularStatement()
        {
            var movie1 = new RegularMovie("Plan 9 from Outer Space");
            _customer.AddRental(new Rental(movie1, 1));
            var movie2 = new RegularMovie("8 1/2");
            _customer.AddRental(new Rental(movie2, 2));
            var movie3 = new RegularMovie("Eraserhead");
            _customer.AddRental(new Rental(movie3, 3));

            _customer.Statement();

            Assert.Equal("Rental Record for Fred\n" +
                         "\tPlan 9 from Outer Space\t2.0\n" +
                         "\t8 1/2\t2.0\n" +
                         "\tEraserhead\t3.5\n" +
                         "You owed 7.5\n" +
                         "You earned 3 frequent renter points\n",
                _customer.BuildStatementString());
        }

    }
}