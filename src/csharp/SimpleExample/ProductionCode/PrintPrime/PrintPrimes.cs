using System;

namespace ProductionCode.PrintPrime
{
    public class PrintPrimes
    {
        public void Run()
        {
            const int numberOfPrimes = 1000;
            const int linesPerPage = 50;
            const int columns = 4;
            const int ordmax = 30;
            int[] primes = new int[numberOfPrimes + 1];
            int candidate;
            int primeIndex;
            bool possiblyPrime;
            int ord;
            int square;
            int n;
            int[] multiples = new int[ordmax + 1];

            candidate = 1;
            primeIndex = 1;
            primes[1] = 2;
            ord = 2;
            square = 9;

            while (primeIndex < numberOfPrimes)
            {
                do
                {
                    candidate = candidate + 2;
                    if (candidate == square)
                    {
                        ord = ord + 1;
                        square = primes[ord] * primes[ord];
                        multiples[ord - 1] = candidate;
                    }
                    n = 2;
                    possiblyPrime = true;
                    while (n < ord && possiblyPrime)
                    {
                        while (multiples[n] < candidate)
                            multiples[n] = multiples[n] + primes[n] + primes[n];
                        if (multiples[n] == candidate)
                            possiblyPrime = false;
                        n = n + 1;
                    }
                } while (!possiblyPrime);
                primeIndex = primeIndex + 1;
                primes[primeIndex] = candidate;
            }

            var pageNumber = 1;
            var pageOffset = 1;
            while (pageOffset <= numberOfPrimes)
            {
                Console.WriteLine("The First " + numberOfPrimes +
                                  " Prime Numbers --- Page " + pageNumber);
                Console.WriteLine("");
                int rowOffset;
                for (rowOffset = pageOffset; rowOffset < pageOffset + linesPerPage; rowOffset++)
                {
                    int column;
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