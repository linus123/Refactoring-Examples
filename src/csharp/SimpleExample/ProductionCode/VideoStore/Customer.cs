using System.Collections.Generic;
using System.Linq;

namespace ProductionCode.VideoStore
{
    public class Customer
    {
        private readonly List<Rental> _rentals = new List<Rental>();
        private readonly List<decimal> _thisAmounts = new List<decimal>();

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

        public void Statement()
        {
            SetTotalAmounts();

            SetFrequentRenterPoints();
        }

        private void SetTotalAmounts()
        {
            TotalAmount = 0;

            foreach (var rental in _rentals)
            {
                decimal thisAmount = rental.AddAmount();

                // show figures for this rental
                TotalAmount += thisAmount;

                _thisAmounts.Add(thisAmount);
            }
        }

        public void SetFrequentRenterPoints()
        {
            foreach (var rental in _rentals)
            {
                FrequentRenterPoints += rental.GetFrequentRenterPoints();
            }
        }
        public string BuildStatementString()
        {
            var result = "Rental Record for " + Name + "\n";

            for (int i = 0; i < _rentals.Count(); i++)
            {
                result += $"\t{_rentals[i].GetTitle()}\t" + $"{_thisAmounts[i]:F1}\n";
            }

            // add footer lines
            result += $"You owed " + $"{TotalAmount:F1}\n";
            result += $"You earned {FrequentRenterPoints} frequent renter points\n";
            return result;
        }


    }
}