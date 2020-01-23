using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionCode.PrintPrime
{
    public class CalculatePrimes
    {
        public int[] CalculatePrime(int numberOfPrimes)
        {
            int[] primes = new int[numberOfPrimes + 1];
            int candidate;
            int primeIndex;
            bool possiblyPrime;
            int ord;
            int square;
            int n;
            const int ordmax = 30;
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

            return primes;
        }
    }
}
