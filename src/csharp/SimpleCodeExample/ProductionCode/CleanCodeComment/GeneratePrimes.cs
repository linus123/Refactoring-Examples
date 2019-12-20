/**
    * This class Generates prime numbers up to a user specified
    * maximum.  The algorithm used is the Sieve of Eratosthenes.
    * <p>
    * Eratosthenes of Cyrene, b. c. 276 BC, Cyrene, Libya --
    * d. c. 194, Alexandria.  The first man to calculate the
    * circumference of the Earth.  Also known for working on
    * calendars with leap years and ran the library at Alexandria.
    * <p>
    * The algorithm is quite simple.  Given an array of integers
    * starting at 2.  Cross out all multiples of 2.  Find the next
    * uncrossed integer, and cross out all of its multiples.
    * Repeat untilyou have passed the square root of the maximum
    * value.
    *
    * @author Alphonse
    * @version 13 Feb 2002 atp
    */

using System;

namespace ProductionCode.CleanCodeComment
{
    public class GeneratePrimes
    {
        public static int[] Generate(int limit)
        {
            if (limit >= 2)
                return GetPrimeNumbers(limit);

            return new int[0];
        }

        private static int[] GetPrimeNumbers(int limit)
        {
            int arraySize = limit + 1;
            bool[] f = new bool[arraySize];
            // initialize array to true.
            for (var i = 0; i < arraySize; i++)
                f[i] = true;

            // get rid of known non-primes
            f[0] = f[1] = false;

            // sieve
            int j;
            for (var i = 2; i < Math.Sqrt(arraySize) + 1; i++)
            {
                if (f[i]) // if i is uncrossed, cross its multiples.
                {
                    for (j = 2 * i; j < arraySize; j += i)
                        f[j] = false; // multiple is not prime
                }
            }

            // how many primes are there?
            int count = 0;
            for (var i = 0; i < arraySize; i++)
            {
                if (f[i])
                    count++; // bump count.
            }

            int[] primes = new int[count];

            int i1;
            int j1;
            // move the primes into the result
            for (i1 = 0, j1 = 0; i1 < arraySize; i1++)
            {
                if (f[i1]) // if prime
                    primes[j1++] = i1;
            }

            return primes; // return the primes
        }
    }
}