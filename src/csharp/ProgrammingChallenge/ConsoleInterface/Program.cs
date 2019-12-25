using System;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var line = Console.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                // Parse line here
                Console.WriteLine($"GotLine : {line}");
                // Parse line here

                line = Console.ReadLine();
            }
        }
    }
}
