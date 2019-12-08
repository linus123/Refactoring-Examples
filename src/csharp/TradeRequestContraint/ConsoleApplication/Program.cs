using System;
using System.Linq;
using SharedKernel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var preferencePrimaryOnly = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = true,
                IsHeavilyTradeFilterActive = false,
                IsCapacityEncumberedSharesFilterActive = false,
            };

            var preferenceHeavilyTradedOnly = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = false,
                IsHeavilyTradeFilterActive = true,
                StockHeavilyTradeDay = 2,
                StockHeavilyTradeVolume = 0.9m,
                IsCapacityEncumberedSharesFilterActive = false,
            };

            var preferenceEncumberedOnly = new TradeFilterPreference()
            {
                IsPrimaryFilterActive = false,
                IsHeavilyTradeFilterActive = false,
                IsCapacityEncumberedSharesFilterActive = true,
            };

            var stock01 = new Stock()
            {
                StockId = "0001",
                PriceInUsd = 600
            };

            stock01.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var stock02 = new Stock()
            {
                StockId = "0002",
                PriceInUsd = 54
            };

            var tradeRequests = new TradeRequest[]
            {
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    TradeRequestId = 500,
                    OriginalCapacityQuantity = 600,
                    Stock = stock01,
                    TradeFilterPreference = preferencePrimaryOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    TradeRequestId = 501,
                    OriginalCapacityQuantity = 100,
                    HoldingsQuantity = 30,
                    EncumberedQuantity = 20,
                    Stock = stock01,
                    TradeFilterPreference = preferenceHeavilyTradedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    TradeRequestId = 502,
                    OriginalCapacityQuantity = 1000,
                    Stock = stock02,
                    TradeFilterPreference = preferenceEncumberedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    TradeRequestId = 502,
                    OriginalCapacityQuantity = 1000,
                    Stock = stock02,
                    TradeFilterPreference = preferenceEncumberedOnly
                },
            };

            var tradeRequestCollection = new TradeRequestCollection(
                tradeRequests);

            tradeRequestCollection.ApplyFilters();

            foreach (var tradeRequest in tradeRequests)
            {
                Console.WriteLine($"Filter Report for TradeRequestId '{tradeRequest.TradeRequestId}' with StockId '{tradeRequest.Stock.StockId}'");
                Console.WriteLine($"\tStarting Quantity: '{tradeRequest.OriginalCapacityQuantity}' and Starting Amount '{tradeRequest.OriginalCapacityQuantity * tradeRequest.Stock.PriceInUsd}'");

                if (tradeRequest.Constraints.Any())
                {
                    foreach (var constraint in tradeRequest.Constraints)
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
