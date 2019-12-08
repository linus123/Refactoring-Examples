using System;
using System.Linq;
using SharedKernel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var profile = new Profile()
            {
                IsPrimaryConstraintActive = false,
                IsBlockHeavilyTradeConstraintActive = false,
                BlockHeavilyTradeDay = 2,
                BlockHeavilyTradeVolume = 0.9m,
                IsCapacityEncumberedSharesConstraintActive = true,
            };

            var block = new Block()
            {
                StockId = "0001",
                PriceInUsd = 600
            };

            block.SetVolumes(
                new decimal[] { 2000, 2100, 2200, 2300, 2400 },
                new decimal[] { 1000, 1100, 1200, 1300, 1400 });

            var orderCapacities = new OrderCapacity[]
            {
                new OrderCapacity()
                {
                    TradeSide = TradeSide.Sell,
                    OrderId = 500,
                    OriginalCapacityQuantity = 100,
                    HoldingsQty = 30,
                    EncumberedQty = 20,
                    Block = block,
                    Profile = profile
                }
            };

            var orderCapacityCollection = new OrderCapacityCollection(
                orderCapacities);

            orderCapacityCollection.ApplyConstraints();

            foreach (var orderCapacity in orderCapacities)
            {
                Console.WriteLine($"Constraint Report for OrderId '{orderCapacity.OrderId}' with StockId '{orderCapacity.Block.StockId}'");
                Console.WriteLine($"\tStarting Quantity: '{orderCapacity.OriginalCapacityQuantity}' and Starting Amount '{orderCapacity.OriginalCapacityQuantity * orderCapacity.Block.PriceInUsd}'");

                if (orderCapacity.Constraints.Any())
                {
                    foreach (var constraint in orderCapacity.Constraints)
                    {
                        Console.WriteLine($"\t\tConstraint '{constraint.ConstraintType}' applied with Quantity '{constraint.ConstrainedQty}' and Amount '{constraint.ConstrainedAmt}'");
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
