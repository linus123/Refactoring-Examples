using System;
using ProductionCode.FundTradeHistory;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RefactoringExample;integrated security=true;Persist Security Info=True";

            var tradeHistoryRepository = new TradeHistoryRepository(connectionString);

            var tradeVolumeHistories = tradeHistoryRepository.GetTradeVolumeHistories(
                new DateTime(2010, 1, 1), 
                new Guid[0]);

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
