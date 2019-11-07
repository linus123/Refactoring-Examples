namespace VideoStore
{
    public class StatementCreator
    {
        private readonly Customer _customer;

        public StatementCreator(
            Customer customer)
        {
            _customer = customer;
        }

        public string GetStatement()
        {
            return GetHeader()
                + GetBody()
                + GetFooter();
        }

        private string GetFooter()
        {
            var result = $"Amount owed is {_customer.GetTotalAmount():F1}\n"
                + $"You earned {_customer.GetFrequentRenterPoints()} frequent renter points";

            return result;
        }

        private string GetBody()
        {
            var result = "";

            _customer.ForEachRental(rental =>
            {
                result += GetFigures(rental);
            });

            return result;
        }

        private static string GetFigures(Rental rental)
        {
            return  $"\t{rental.GetTitle()}\t"
                    + $"{rental.GetAmount():F1}\n";
        }

        private string GetHeader()
        {
            return "Rental Record for " + _customer.Name + "\n";
        }
    }
}