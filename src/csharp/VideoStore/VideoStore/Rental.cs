namespace VideoStore
{
    public class Rental
    {
        public Movie Movie { get; }
        public int DaysRented { get; }

        public Rental(Movie movie, int daysRented)
        {
            Movie = movie;
            DaysRented = daysRented;
        }

        public decimal GetAmount()
        {
            decimal thisAmount = 0;

            switch (Movie.PriceCode)
            {
                case Movie.Regular:
                    thisAmount += 2;
                    if (DaysRented > 2)
                        thisAmount += (DaysRented - 2) * 1.5m;
                    break;
                case Movie.NewRelease:
                    thisAmount += DaysRented * 3;
                    break;
                case Movie.Childrens:
                    thisAmount += 1.5m;
                    if (DaysRented > 3)
                        thisAmount += (DaysRented - 3) * 1.5m;
                    break;
            }

            return thisAmount;
        }
    }
}