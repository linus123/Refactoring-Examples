using System.Collections.Generic;
using System.Linq;

namespace VideoStore
{
    public class Customer
    {
        private readonly List<Rental> _rentals = new List<Rental>();

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
            
            foreach (var rental in _rentals)
            {
                var thisAmount = rental.GetAmount();

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
            result += $"Amount owed is {totalAmount:F1}\n";
            result += $"You earned {frequentRenterPoints} frequent renter points";
            return result;
        }
    }
}