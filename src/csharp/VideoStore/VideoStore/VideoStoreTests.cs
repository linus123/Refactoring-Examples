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

        [Fact]
        public void SingleNewReleaseStatement()
        {
            var movie = new Movie("The Cell", Movie.NewRelease);
            _customer.AddRental(new Rental(movie, 3));
            _customer.TotalAmount.Should().BeApproximately(9.0m, 0.0001m);
            _customer.FrequentRenterPoints.Should().Be(2);
        }

        [Fact]
        public void DualNewReleaseStatement()
        {
            var movie1 = new Movie("The Cell", Movie.NewRelease);
            _customer.AddRental(new Rental(movie1, 3));
            var movie2 = new Movie("The Tigger Movie", Movie.NewRelease);
            _customer.AddRental(new Rental(movie2, 3));
            Assert.Equal("Rental Record for Fred\n" +
                         "\tThe Cell\t9.0\n" +
                         "\tThe Tigger Movie\t9.0\n" +
                         "You owed 18.0\n" +
                         "You earned 4 frequent renter points\n",
                _customer.Statement());
        }

        [Fact]
        public void SingleChildrensStatement()
        {
            var movie = new Movie("The Tigger Movie", Movie.Childrens);
            _customer.AddRental(new Rental(movie, 3));
            Assert.Equal("Rental Record for Fred\n" +
                         "\tThe Tigger Movie\t1.5\n" +
                         "You owed 1.5\n" +
                         "You earned 1 frequent renter points\n",
                _customer.Statement());
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