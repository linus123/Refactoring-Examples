using System;
using System.Linq;
using SharedKernel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var profilePrimaryOnly = new TradeFilterPreference()
            {
                IsPrimaryConstraintActive = true,
                IsBlockHeavilyTradeConstraintActive = false,
                IsCapacityEncumberedSharesConstraintActive = false,
            };

            var profileHeavilyTradedOnly = new TradeFilterPreference()
            {
                IsPrimaryConstraintActive = false,
                IsBlockHeavilyTradeConstraintActive = true,
                BlockHeavilyTradeDay = 2,
                BlockHeavilyTradeVolume = 0.9m,
                IsCapacityEncumberedSharesConstraintActive = false,
            };

            var profileEncumberedOnly = new TradeFilterPreference()
            {
                IsPrimaryConstraintActive = false,
                IsBlockHeavilyTradeConstraintActive = false,
                IsCapacityEncumberedSharesConstraintActive = true,
            };

            var block01 = new Stock()
            {
                StockId = "0001",
                PriceInUsd = 600
            };

            block01.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var block02 = new Stock()
            {
                StockId = "0002",
                PriceInUsd = 54
            };

            var orderCapacities = new TradeRequest[]
            {
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    TradeRequestId = 500,
                    OriginalCapacityQuantity = 600,
                    Stock = block01,
                    TradeFilterPreference = profilePrimaryOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    TradeRequestId = 501,
                    OriginalCapacityQuantity = 100,
                    HoldingsQuantity = 30,
                    EncumberedQuantity = 20,
                    Stock = block01,
                    TradeFilterPreference = profileHeavilyTradedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    TradeRequestId = 502,
                    OriginalCapacityQuantity = 1000,
                    Stock = block02,
                    TradeFilterPreference = profileEncumberedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    TradeRequestId = 502,
                    OriginalCapacityQuantity = 1000,
                    Stock = block02,
                    TradeFilterPreference = profileEncumberedOnly
                },
            };

            var orderCapacityCollection = new TradeRequestCollection(
                orderCapacities);

            orderCapacityCollection.ApplyConstraints();

            foreach (var orderCapacity in orderCapacities)
            {
                Console.WriteLine($"Filter Report for TradeRequestId '{orderCapacity.TradeRequestId}' with StockId '{orderCapacity.Stock.StockId}'");
                Console.WriteLine($"\tStarting Quantity: '{orderCapacity.OriginalCapacityQuantity}' and Starting Amount '{orderCapacity.OriginalCapacityQuantity * orderCapacity.Stock.PriceInUsd}'");

                if (orderCapacity.Constraints.Any())
                {
                    foreach (var constraint in orderCapacity.Constraints)
                    {
                        Console.WriteLine($"\t\tConstraint '{constraint.FilterType}' applied with Quantity '{constraint.FilteredQuantity}' and Amount '{constraint.FilteredAmount}'");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tNo Constraints");
                }

                Console.WriteLine($"\tFinal Available Quantity: {orderCapacity.AvailableCapacityQuantity}");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to end...");
            Console.ReadKey();
        }
    }
}
