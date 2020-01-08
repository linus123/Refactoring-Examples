namespace ProductionCode.VideoStore
{
    public abstract class Movie : IMovie
    {
        public string Title { get; }
        public abstract decimal AddAmount(int daysRented);

        public abstract int GetFrequentRenterPoints(int daysRented);
        
        protected Movie(string title)
        {
            Title = title;
        }
    }
}