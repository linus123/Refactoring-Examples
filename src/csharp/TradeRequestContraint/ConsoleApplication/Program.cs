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
                IsCapacityEncumberedSharesConstraintActive = false,
                IsBlockHeavilyTradeConstraintActive = true,
                BlockHeavilyTradeDay = 2,
                BlockHeavilyTradeVolume = 0.9m
            };

            var block = new Block()
            {
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
                    Block = block,
                    HoldingsQty = 30,
                    EncumberedQty = 20
                }
            };

            var orderCapacityCollection = new OrderCapacityCollection(
                orderCapacities);

            orderCapacityCollection.ApplyConstraints(profile);

            Console.WriteLine("Constraint Report");

            foreach (var orderCapacity in orderCapacities)
            {
                Console.WriteLine($"\tOrderId: {orderCapacity.OrderId}");

                if (orderCapacity.Constraints.Any())
                {
                    foreach (var constraint in orderCapacity.Constraints)
                    {
                        Console.WriteLine($"\t\tConstraint '{constraint.ConstraintType}'");
                        Console.WriteLine($"\t\t\t Quantity {constraint.ConstrainedQty}");
                        Console.WriteLine($"\t\t\t Amount {constraint.ConstrainedAmt}");
                        Console.WriteLine($"\t\t\t Available Quantity {constraint.AvailQty}");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tNo Constraints");
                }

            }

            Console.WriteLine("Press any key to end...");
            Console.ReadKey();
        }
    }
}
