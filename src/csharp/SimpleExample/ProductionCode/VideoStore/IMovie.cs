namespace ProductionCode.VideoStore
{
    
    public interface IMovie
    {
         string Title { get; }
         decimal AddAmount(int daysRented);
         int GetFrequentRenterPoints(int daysRented);
    }

    public class RegularMovie : Movie
    {
      public override decimal AddAmount(int daysRented)
        {
            decimal amount = 0;
            amount += 2;
            if (daysRented > 2)
                amount += (daysRented - 2) * 1.5m;
            return amount;

        }

      public override int GetFrequentRenterPoints(int daysRented)
      {
          return 1;
      }

      public RegularMovie(string title) : base(title)
        {
            
        }
    }

    public class ChildrensMovie : Movie
    {
        public override decimal AddAmount(int daysRented)
        {
            decimal amount = 1.5m;
            if (daysRented > 3)
                amount += (daysRented - 3) * 1.5m;
            return amount;
        }

        public override int GetFrequentRenterPoints(int daysRented)
        {
            return 1;
        }

        public ChildrensMovie(string title) : base(title)
        {
        }
    }

    public class NewReleaseMovie : Movie
    {
       public override decimal AddAmount(int daysRented)
        {
            decimal amount = daysRented * 3;
            return amount;

        }

       public override int GetFrequentRenterPoints(int daysRented)
       {
           if (daysRented > 1)
               return 2;

           return 1;
       }

        public NewReleaseMovie(string title) : base(title)
        {
        }
    }
}
