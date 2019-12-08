using System;
using System.Linq;
using SharedKernel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var profilePrimaryOnly = new Profile()
            {
                IsPrimaryConstraintActive = true,
                IsBlockHeavilyTradeConstraintActive = false,
                IsCapacityEncumberedSharesConstraintActive = false,
            };

            var profileHeavilyTradedOnly = new Profile()
            {
                IsPrimaryConstraintActive = false,
                IsBlockHeavilyTradeConstraintActive = true,
                BlockHeavilyTradeDay = 2,
                BlockHeavilyTradeVolume = 0.9m,
                IsCapacityEncumberedSharesConstraintActive = false,
            };

            var profileEncumberedOnly = new Profile()
            {
                IsPrimaryConstraintActive = false,
                IsBlockHeavilyTradeConstraintActive = false,
                IsCapacityEncumberedSharesConstraintActive = true,
            };

            var block01 = new Block()
            {
                StockId = "0001",
                PriceInUsd = 600
            };

            block01.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var block02 = new Block()
            {
                StockId = "0002",
                PriceInUsd = 54
            };

            var orderCapacities = new TradeRequest[]
            {
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    OrderId = 500,
                    OriginalCapacityQuantity = 600,
                    Block = block01,
                    Profile = profilePrimaryOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    OrderId = 501,
                    OriginalCapacityQuantity = 100,
                    HoldingsQty = 30,
                    EncumberedQty = 20,
                    Block = block01,
                    Profile = profileHeavilyTradedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Buy,
                    OrderId = 502,
                    OriginalCapacityQuantity = 1000,
                    Block = block02,
                    Profile = profileEncumberedOnly
                },
                new TradeRequest()
                {
                    TradeSide = TradeSide.Sell,
                    OrderId = 502,
                    OriginalCapacityQuantity = 1000,
                    Block = block02,
                    Profile = profileEncumberedOnly
                },
            };

            var orderCapacityCollection = new TradeRequestCollection(
                orderCapacities);

            orderCapacityCollection.ApplyConstraints();

            foreach (var orderCapacity in orderCapacities)
            {
                Console.WriteLine($"Filter Report for OrderId '{orderCapacity.OrderId}' with StockId '{orderCapacity.Block.StockId}'");
                Console.WriteLine($"\tStarting Quantity: '{orderCapacity.OriginalCapacityQuantity}' and Starting Amount '{orderCapacity.OriginalCapacityQuantity * orderCapacity.Block.PriceInUsd}'");

                if (orderCapacity.Constraints.Any())
                {
                    foreach (var constraint in orderCapacity.Constraints)
                    {
                        Console.WriteLine($"\t\tConstraint '{constraint.FilterType}' applied with Quantity '{constraint.FilteredQuantity}' and Amount '{constraint.ConstrainedAmt}'");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tNo Constraints");
                }

                Console.WriteLine($"\tFinal Available Quantity: {orderCapacity.AvailCapacityQty}");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to end...");
            Console.ReadKey();
        }
    }
}
