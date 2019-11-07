using System;
using System.Collections.Generic;

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

        public string GetStatement()
        {
            return new StatementCreator(this)
                .GetStatement();
        }

        public decimal GetTotalAmount()
        {
            decimal amount = 0;

            foreach (var rental in _rentals)
                amount += rental.GetAmount();

            return amount;
        }

        public int GetFrequentRenterPoints()
        {
            var points = 0;

            foreach (var r in _rentals)
                points += r.GetFrequentRenterPoints();

            return points;
        }

        public void ForEachRental(
            Action<Rental> act)
        {
            foreach (var r in _rentals)
            {
                act(r);
            }
        }
    }
}