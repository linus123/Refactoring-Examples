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
            int pageNumber;
            int pageOffset;
            int rowOffset;
            int column;
            int candiate;
            int primeIndex;
            bool possiblyPrime;
            int ord;
            int square;
            int n;
            int[] multiples = new int[ordmax + 1];

            candiate = 1;
            primeIndex = 1;
            primes[1] = 2;
            ord = 2;
            square = 9;

            while (primeIndex < numberOfPrimes)
            {
                do
                {
                    candiate = candiate + 2;
                    if (candiate == square)
                    {
                        ord = ord + 1;
                        square = primes[ord] * primes[ord];
                        multiples[ord - 1] = candiate;
                    }
                    n = 2;
                    possiblyPrime = true;
                    while (n < ord && possiblyPrime)
                    {
                        while (multiples[n] < candiate)
                            multiples[n] = multiples[n] + primes[n] + primes[n];
                        if (multiples[n] == candiate)
                            possiblyPrime = false;
                        n = n + 1;
                    }
                } while (!possiblyPrime);
                primeIndex = primeIndex + 1;
                primes[primeIndex] = candiate;
            }
            {
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
}