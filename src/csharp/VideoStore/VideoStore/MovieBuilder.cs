namespace VideoStore
{
    public class MovieBuilder
    {
        private static int _movieCounter = 1;
        private string _title;

        public MovieBuilder()
        {
            _title = "Movie" + _movieCounter.ToString("D3");
            _movieCounter++;
        }

        public MovieBuilder WithTitle(
            string title)
        {
            _title = title;

            return this;
        }

        public RegularMovie CreateRegular()
        {
            return new RegularMovie(_title);
        }

        public NewReleaseMovie CreateNewRelease()
        {
            return new NewReleaseMovie(_title);
        }

        public ChildrensMovie CreateChildrens()
        {
            return new ChildrensMovie(_title);
        }


    }
}