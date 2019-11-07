using FluentAssertions;
using Xunit;

namespace VideoStore
{
    public class VideoStoreTests
    {
        private Customer _customer;
        private readonly Movie _newRelease1 = new Movie("New Release 1", Movie.NewRelease);
        private readonly Movie _newRelease2 = new Movie("New Release 2", Movie.NewRelease);
        private readonly Movie _childrens = new Movie("Childrens", Movie.Childrens);
        private readonly Movie _regular1 = new Movie("Regular 1", Movie.Regular);
        private readonly Movie _regular2 = new Movie("Regular 2", Movie.Regular);
        private readonly Movie _regular3 = new Movie("Regular 3", Movie.Regular);

        public VideoStoreTests()
        {
            _customer = new Customer("Customer Name");
        }

        private void AssertAmountAndPointsForStatement(double expectedAmount, int expectedPoints)
        {
            var statement = _customer.Statement();
            statement.Should().Contain($"Amount owed is {expectedAmount:F1}");
            statement.Should().Contain($"You earned {expectedPoints} frequent renter points");
        }

        [Fact]
        public void TestSingleNewReleaseStatement()
        {
            _customer.AddRental(new Rental(_newRelease1, 3));
            AssertAmountAndPointsForStatement(9.0, 2);
        }

        [Fact]
        public void TestDualNewReleaseStatement()
        {
            _customer.AddRental(new Rental(_newRelease1, 3));
            _customer.AddRental(new Rental(_newRelease2, 3));
            AssertAmountAndPointsForStatement(18.0, 4);
        }

        [Fact]
        public void TestSingleChildrensStatement()
        {
            _customer.AddRental(new Rental(_childrens, 3));
            AssertAmountAndPointsForStatement(1.5, 1);
        }

        [Fact]
        public void TestMultipleRegularStatement()
        {
            _customer.AddRental(new Rental(_regular1, 1));
            _customer.AddRental(new Rental(_regular2, 2));
            _customer.AddRental(new Rental(_regular3, 3));
            AssertAmountAndPointsForStatement(7.5, 3);
        }

        [Fact]
        public void TestRentalStatementFormat()
        {
            _customer.AddRental(new Rental(_regular1, 1));
            _customer.AddRental(new Rental(_regular2, 2));
            _customer.AddRental(new Rental(_regular3, 3));

            const string expectedStatement = "Rental Record for Customer Name\n" +
                                             "\tRegular 1\t2.0\n" +
                                             "\tRegular 2\t2.0\n" +
                                             "\tRegular 3\t3.5\n" +
                                             "Amount owed is 7.5\n" +
                                             "You earned 3 frequent renter points";

            _customer.Statement().Should().Be(expectedStatement);
        }
    }
}