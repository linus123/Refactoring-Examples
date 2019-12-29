using System;
using ProductionCode.FundTradeHistory;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RefactoringExample;integrated security=true;Persist Security Info=True";

            var tradeDataTableGateway = new TradeDataTableGateway(connectionString);

            var stockIds = tradeDataTableGateway.GetStockIds();

            var tradeHistoryRepository = new TradeHistoryRepository(connectionString);

            var tradeDate = DateTime.Now;

            var tradeVolumeHistories = tradeHistoryRepository.GetTradeVolumes(tradeDate, stockIds);

            foreach (var tradeVolumeHistory in tradeVolumeHistories)
            {
                tradeVolumeHistory.Accumulate10DayVolume();

                Console.WriteLine($"{tradeVolumeHistory.StockId}, {tradeVolumeHistory.GetAccumulatedDayVolume(1)}, {tradeVolumeHistory.GetAccumulatedDayVolume(2)}, {tradeVolumeHistory.GetAccumulatedDayVolume(3)}");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
