namespace ProductionCode.VideoStore
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

        public int GetPriceCode()
        {
            return Movie.PriceCode;
        }

        public string GetTitle()
        {
            return Movie.Title;
        }
    }
}