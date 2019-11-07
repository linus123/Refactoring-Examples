using FluentAssertions;
using Xunit;

namespace VideoStore
{
    public class VideoStoreTests
    {
        private readonly Customer _customer;

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
            var newRelease = new MovieBuilder()
                .WithPriceCodeAsNewRelease()
                .Create();

            _customer.AddRental(new Rental(newRelease, 3));
            AssertAmountAndPointsForStatement(9.0, 2);
        }

        [Fact]
        public void TestDualNewReleaseStatement()
        {
            var newRelease1 = new MovieBuilder()
                .WithPriceCodeAsNewRelease()
                .Create();

            var newRelease2 = new MovieBuilder()
                .WithPriceCodeAsNewRelease()
                .Create();

            _customer.AddRental(new Rental(newRelease1, 3));
            _customer.AddRental(new Rental(newRelease2, 3));
            AssertAmountAndPointsForStatement(18.0, 4);
        }

        [Fact]
        public void TestSingleChildrensStatement()
        {
            var childrens = new MovieBuilder()
                .WithPriceCodeAsChildrens()
                .Create();

            _customer.AddRental(new Rental(childrens, 3));
            AssertAmountAndPointsForStatement(1.5, 1);
        }

        [Fact]
        public void TestMultipleRegularStatement()
        {
            var regular1 = new MovieBuilder()
                .WithPriceCodeAsRegular()
                .Create();

            var regular2 = new MovieBuilder()
                .WithPriceCodeAsRegular()
                .Create();

            var regular3 = new MovieBuilder()
                .WithPriceCodeAsRegular()
                .Create();

            _customer.AddRental(new Rental(regular1, 1));
            _customer.AddRental(new Rental(regular2, 2));
            _customer.AddRental(new Rental(regular3, 3));

            AssertAmountAndPointsForStatement(7.5, 3);
        }

        [Fact]
        public void TestRentalStatementFormat()
        {
            var regular1 = new MovieBuilder()
                .WithTitle("Regular 1")
                .WithPriceCodeAsRegular()
                .Create();

            var regular2 = new MovieBuilder()
                .WithTitle("Regular 2")
                .WithPriceCodeAsRegular()
                .Create();

            var regular3 = new MovieBuilder()
                .WithTitle("Regular 3")
                .WithPriceCodeAsRegular()
                .Create();

            _customer.AddRental(new Rental(regular1, 1));
            _customer.AddRental(new Rental(regular2, 2));
            _customer.AddRental(new Rental(regular3, 3));

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