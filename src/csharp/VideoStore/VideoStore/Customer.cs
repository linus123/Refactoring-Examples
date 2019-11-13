using System.Collections;
using System.Linq;

namespace VideoStore
{
    public class Customer
    {
        private readonly ArrayList _rentals = new ArrayList();
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void AddRental(Rental rental)
        {
            _rentals.Add(rental);
        }

        public string Statement()
        {
            decimal totalAmount = 0;
            var frequentRenterPoints = 0;
            var result = "Rental Record for " + Name + "\n";
            foreach (var rental in _rentals.Cast<Rental>())
            {
                decimal thisAmount = 0;
                switch (rental.Movie.PriceCode)
                {
                    case Movie.Regular:
                        thisAmount += 2;
                        if (rental.DaysRented > 2)
                            thisAmount += (rental.DaysRented - 2) * 1.5m;
                        break;
                    case Movie.NewRelease:
                        thisAmount += rental.DaysRented * 3;
                        break;
                    case Movie.Childrens:
                        thisAmount += 1.5m;
                        if (rental.DaysRented > 3)
                            thisAmount += (rental.DaysRented - 3) * 1.5m;
                        break;
                    default:
                        break;
                }

                // add frequent renter points
                frequentRenterPoints++;
                // add bonus for a two day new release rental
                if ((rental.Movie.PriceCode == Movie.NewRelease) &&
                    rental.DaysRented > 1)
                    frequentRenterPoints++;

                // show figures for this rental
                result += $"\t{rental.Movie.Title}\t" + $"{thisAmount:F1}\n";
                totalAmount += thisAmount;
            }

            // add footer lines
            result += $"You owed " + $"{totalAmount:F1}\n";
            result += $"You earned {frequentRenterPoints} frequent renter points\n";
            return result;
        }
    }
}