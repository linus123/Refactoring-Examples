using FluentAssertions;
using ProductionCode.TestDrivenDev;
using Xunit;

namespace TestCode.TestDrivenDev
{
    public class MoneyTests
    {
        [Fact]
        public void DollarMultiplication()
        {
            Money five = Money.Dollar(5);
            five.Times(2).Should().Be(Money.Dollar(10));
            five.Times(3).Should().Be(Money.Dollar(15));
        }

        [Fact]
        public void EqualityDollar()
        {
            var five = Money.Dollar(5);

            five.Equals(Money.Dollar(5)).Should().BeTrue();
            five.Equals(Money.Dollar(6)).Should().BeFalse();
        }

        [Fact]
        public void FrancMultiplication()
        {
            var five = Money.Franc(5);

            five.Times(2).Should().Be(Money.Franc(10));
            five.Times(3).Should().Be(Money.Franc(15));
        }

        [Fact]
        public void EqualityFranc()
        {
            var five = Money.Franc(5);

            five.Equals(Money.Franc(5)).Should().BeTrue();
            five.Equals(Money.Franc(6)).Should().BeFalse();
        }

        [Fact]
        public void EqualityDollarAndFrank()
        {
            Money.Franc(5).Should().NotBe(Money.Dollar(5));
        }

    }
}