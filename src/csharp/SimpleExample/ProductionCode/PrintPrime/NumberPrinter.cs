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
            var pageOffset = 1;
            while (pageOffset <= numberOfPrimes)
            {
                Console.WriteLine(GetPageHeader(numberOfPrimes, pageNumber));
                Console.WriteLine("");
                for (var rowOffset = pageOffset; rowOffset < pageOffset + LinesPerPage; rowOffset++)
                {
                    var line = GetSingleLine(primes, numberOfPrimes, rowOffset);
                    Console.WriteLine(line);
                }

                Console.WriteLine();
                pageNumber = pageNumber + 1;
                pageOffset = pageOffset + LinesPerPage * Columns;
            }
        }

        private string GetSingleLine(
            int[] primes,
            int numberOfPrimes,
            int rowOffset)
        {
            string line = "";

            for (var column = 0; column < Columns; column++)
                if (rowOffset + column * LinesPerPage <= numberOfPrimes)
                    line += string.Format("{0, 10}", primes[rowOffset + column * LinesPerPage]);

            return line;
        }

        private static string GetPageHeader(int numberOfPrimes, int pageNumber)
        {
            return "The First " + numberOfPrimes +
                   " Prime Numbers --- Page " + pageNumber;
        }
    }
}