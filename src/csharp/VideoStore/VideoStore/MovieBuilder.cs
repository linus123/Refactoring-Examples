namespace VideoStore
{
    public class MovieBuilder
    {
        private static int _movieCounter = 1;

        private int _priceCode;
        private string _title;

        public MovieBuilder()
        {
            _priceCode = Movie.Regular;
            _title = "Movie" + _movieCounter.ToString("D3");
            _movieCounter++;
        }

        public MovieBuilder WithTitle(
            string title)
        {
            _title = title;

            return this;
        }

        public MovieBuilder WithPriceCodeAsRegular()
        {
            _priceCode = Movie.Regular;

            return this;
        }

        public MovieBuilder WithPriceCodeAsNewRelease()
        {
            _priceCode = Movie.NewRelease;

            return this;
        }

        public MovieBuilder WithPriceCodeAsChildrens()
        {
            _priceCode = Movie.Childrens;

            return this;
        }

        public Movie Create()
        {
            var movie = new Movie(_title, _priceCode);

            return movie;
        }
    }
}