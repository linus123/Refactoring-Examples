using System;
using System.Linq;
using SharedKernel.Persistence;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var tradeRequestCollectionRepository = new TradeRequestCollectionRepository();

            var tradeRequestCollection = tradeRequestCollectionRepository.GetTradeRequests();

            tradeRequestCollection.ApplyFilters();

            foreach (var tradeRequest in tradeRequestCollection.GetTradeRequests())
            {
                Console.WriteLine($"Filter Report for TradeRequestId '{tradeRequest.TradeRequestId}' with StockId '{tradeRequest.Stock.StockId}'");
                Console.WriteLine($"\tStarting Quantity: '{tradeRequest.OriginalCapacityQuantity}' and Starting Amount '{tradeRequest.GetOriginalCapacityAmount()}'");

                if (tradeRequest.Filters.Any())
                {
                    foreach (var constraint in tradeRequest.Filters)
                    {
                        Console.WriteLine($"\t\tFilter '{constraint.FilterType}' applied with Quantity '{constraint.FilteredQuantity}' and Amount '{constraint.FilteredAmount}'");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tNo Filters");
                }

                Console.WriteLine($"\tFinal Available Quantity: {tradeRequest.AvailableCapacityQuantity}");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to end...");
            Console.ReadKey();
        }
    }
}
