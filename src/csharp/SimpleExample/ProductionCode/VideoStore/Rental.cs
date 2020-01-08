namespace ProductionCode.VideoStore
{
    public class Rental
    {
        public IMovie Movie { get; }
        public int DaysRented { get; }

        public Rental(IMovie movie, int daysRented)
        {
            Movie = movie;
            DaysRented = daysRented;
        }

        public string GetTitle()
        {
            return Movie.Title;
        }

        public decimal AddAmount()
        {
            return this.Movie.AddAmount(DaysRented);
        }

        public int GetFrequentRenterPoints()
        {
            return this.Movie.GetFrequentRenterPoints(DaysRented);
        }
    }
}