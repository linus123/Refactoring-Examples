namespace VideoStore
{
    public abstract class Movie
    {
        public const int Childrens = 2;
        public const int Regular = 0;
        public const int NewRelease = 1;

        public string Title { get; }
        public int PriceCode { get; set; }

        protected Movie(string title, int priceCode)
        {
            Title = title;
            PriceCode = priceCode;
        }

        public abstract decimal GetAmount(
            int daysRented);

        public abstract bool ShouldReceiveFrequentRenterPointBonus();
    }

    public class RegularMovie : Movie
    {
        public RegularMovie(
            string title)
            : base(title, Regular)
        {
        }

        public override decimal GetAmount(
            int daysRented)
        {
            decimal amount = 2m;

            if (daysRented > 2)
                amount += (daysRented - 2) * 1.5m;

            return amount;
        }

        public override bool ShouldReceiveFrequentRenterPointBonus()
        {
            return false;
        }
    }

    public class NewReleaseMovie : Movie
    {
        public NewReleaseMovie(
            string title)
            : base(title, NewRelease)
        {
        }

        public override decimal GetAmount(
            int daysRented)
        {
            return daysRented * 3;
        }

        public override bool ShouldReceiveFrequentRenterPointBonus()
        {
            return true;
        }
    }

    public class ChildrensMovie : Movie
    {
        public ChildrensMovie(
            string title)
            : base(title, Childrens)
        {
        }

        public override decimal GetAmount(
            int daysRented)
        {
            decimal amount = 1.5m;

            if (daysRented > 3)
                amount += (daysRented - 3) * 1.5m;

            return amount;
        }

        public override bool ShouldReceiveFrequentRenterPointBonus()
        {
            return false;
        }
    }
}