using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionCode.PrintPrime
{
    class PrintPrimeNumbers
    {
        public void Print(int numberOfPrimes, int linesPerPage, int columns, int[] primes)
        {
            int pageNumber;
            int pageOffset;
            int rowOffset;
            int column;
            pageNumber = 1;
            pageOffset = 1;
            while (pageOffset <= numberOfPrimes)
            {
                Console.WriteLine("The First " + numberOfPrimes +
                                  " Prime Numbers --- Page " + pageNumber);
                Console.WriteLine("");
                for (rowOffset = pageOffset; rowOffset < pageOffset + linesPerPage; rowOffset++)
                {
                    for (column = 0; column < columns; column++)
                        if (rowOffset + column * linesPerPage <= numberOfPrimes)
                            Console.Write("{0, 10}", primes[rowOffset + column * linesPerPage]);
                    Console.WriteLine("");
                }
                Console.WriteLine();
                pageNumber = pageNumber + 1;
                pageOffset = pageOffset + linesPerPage * columns;
            }
        }
    }
}
