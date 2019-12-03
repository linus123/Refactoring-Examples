using System.Collections;
using System.Linq;
//TODO: Separate Calculation from Printing out
//TODO: GetFrequentRenterPoints
//TODO: GetTotalAmount
//TODO: Convert Switch to Polymorphism
//TODO: Convert Array to Generic

namespace VideoStore
{
    public class Customer
    {
        private readonly ArrayList _rentals = new ArrayList();
        public decimal TotalAmount;
        public string Name { get; }
        public int FrequentRenterPoints { get; set; }
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
            TotalAmount = 0;
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

                        // No test coverage on this condition
                        if (rental.DaysRented > 3)
                            thisAmount += (rental.DaysRented - 3) * 1.5m;
                        break;
                    default:
                        break;
                }

                // add frequent renter points

                FrequentRenterPoints++;
                // add bonus for a two day new release rental
                if ((rental.Movie.PriceCode == Movie.NewRelease) &&
                    rental.DaysRented > 1)
                    FrequentRenterPoints++;

                // show figures for this rental
                result += $"\t{rental.Movie.Title}\t" + $"{thisAmount:F1}\n";
                TotalAmount += thisAmount;
            }

            // add footer lines
            result += $"You owed " + $"{TotalAmount:F1}\n";
            result += $"You earned {FrequentRenterPoints} frequent renter points\n";
            return result;
        }
    }
}