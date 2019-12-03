using FluentAssertions;
using Xunit;

namespace VideoStore
{
    public class VideoStoreTests
    {
        private readonly Customer _customer;

        public VideoStoreTests()
        {
            _customer = new Customer("Fred");
        }

        private Movie CreateRelease(int movieRelease)
        {
            return new Movie("SomeMovie", movieRelease);
        }

        [Theory]
        [InlineData(1, 3, 1)]
        [InlineData(3, 9, 2)]
        public void SingleNewReleaseStatement(
            int daysRented, 
            decimal expectedTotalAmount,
            int expectedFrequentRenterPoints)
        {
            var movie = CreateRelease(Movie.NewRelease);
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
            var movie = CreateRelease(Movie.Regular);
            _customer.AddRental(new Rental(movie, daysRented));
            _customer.Statement();
            _customer.TotalAmount.Should().BeApproximately(expectedTotalAmount, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(expectedFrequentRenterPoints);
        }

        [Fact]
        public void DualNewReleaseStatement()
        {
            var movie1 = CreateRelease(Movie.NewRelease);
            _customer.AddRental(new Rental(movie1, 3));
            var movie2 = CreateRelease(Movie.NewRelease);
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
            var movie = CreateRelease(Movie.Childrens);
            _customer.AddRental(new Rental(movie, daysRented));
            _customer.Statement();
            _customer.TotalAmount.Should().BeApproximately(totalAmount, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(frequentRenterPoints);
        }

        [Fact]
        public void MultipleRegularStatement()
        {
            var movie1 = new Movie("Plan 9 from Outer Space", Movie.Regular);
            _customer.AddRental(new Rental(movie1, 1));
            var movie2 = new Movie("8 1/2", Movie.Regular);
            _customer.AddRental(new Rental(movie2, 2));
            var movie3 = new Movie("Eraserhead", Movie.Regular);
            _customer.AddRental(new Rental(movie3, 3));

            Assert.Equal("Rental Record for Fred\n" +
                         "\tPlan 9 from Outer Space\t2.0\n" +
                         "\t8 1/2\t2.0\n" +
                         "\tEraserhead\t3.5\n" +
                         "You owed 7.5\n" +
                         "You earned 3 frequent renter points\n",
                _customer.Statement());
        }

    }
}