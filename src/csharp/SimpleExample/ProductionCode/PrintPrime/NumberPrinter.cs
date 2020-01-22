using System;

namespace ProductionCode.PrintPrime
{
    public class NumberPrinter
    {
        private const int LinesPerPage = 50;
        private const int Columns = 4;

        public void PrintNumbers(
            int[] primes,
            int numberOfPrimes)
        {
            var pageNumber = 1;
            var linesPerPage = 1;
            while (linesPerPage <= numberOfPrimes)
            {
                Console.WriteLine("The First " + numberOfPrimes +
                                  " Prime Numbers --- Page " + pageNumber);
                Console.WriteLine("");
                for (var rowOffset = linesPerPage; rowOffset < linesPerPage + LinesPerPage; rowOffset++)
                {
                    for (var column = 0; column < Columns; column++)
                        if (rowOffset + column * LinesPerPage <= numberOfPrimes)
                            Console.Write("{0, 10}", primes[rowOffset + column * LinesPerPage]);
                    Console.WriteLine("");
                }

                Console.WriteLine();
                pageNumber = pageNumber + 1;
                linesPerPage = linesPerPage + LinesPerPage * Columns;
            }
        }
    }
}