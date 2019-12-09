using FluentAssertions;
using SharedKernel;

namespace UnitTests
{
    public class FilterAssert
    {
        private const decimal Precision = 0.00001m;

        private readonly FilterModel _filter;

        public FilterAssert(
            FilterModel filter)
        {
            _filter = filter;
        }

        public FilterAssert FilteredQuantityShouldBe(
            decimal v)
        {
            _filter.FilteredQuantity.Should().BeApproximately(v, Precision);

            return this;
        }

        public FilterAssert FilterTypeShouldBe(
            string v)
        {
            _filter.FilterType.Should().Be(v);

            return this;
        }

        public FilterAssert OriginalQuantityShouldBe(
            decimal v)
        {
            _filter.OriginalQuantity.Should().BeApproximately(v, Precision);

            return this;
        }

        public FilterAssert AvailableQuantityShouldBe(
            decimal v)
        {
            _filter.AvailableQuantity.Should().BeApproximately(v, Precision);

            return this;
        }

        public FilterAssert FilteredAmountQuantityShouldBe(
            decimal v)
        {
            _filter.FilteredAmount.Should().BeApproximately(v, Precision);

            return this;
        }

        public FilterAssert FilterDescriptionQuantityShouldBe(
            string v)
        {
            _filter.FilterDescription.Should().Be(v);

            return this;
        }

        public FilterAssert IsAppliedShouldBeTrue()
        {
            _filter.IsApplied.Should().BeTrue();

            return this;
        }

        public FilterAssert IsAppliedShouldBeFalse()
        {
            _filter.IsApplied.Should().BeFalse();

            return this;
        }

    }
}