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
            return Movie.GetAmount(DaysRented);
        }

        public int GetFrequentRenterPoints()
        {
            if (ShouldReceiveFrequentRenterPointBonus())
                return 2;

            return 1;
        }

        private bool ShouldReceiveFrequentRenterPointBonus()
        {
            return Movie.ShouldReceiveFrequentRenterPointBonus()
                   && DaysRented > 2;
        }

        public string GetTitle()
        {
            return Movie.Title;
        }

    }
}