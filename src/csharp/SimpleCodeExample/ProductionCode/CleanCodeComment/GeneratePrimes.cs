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
    * Repeat until you have passed the square root of the maximum
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

            var flags = new NumberFlagCollection(arraySize);

            flags.RemoveKnownNonPrimes();

            flags.RemoveAllMultiplesOfTwo();

            var primeCount = flags.GetNumberOfPrimes();

            return CreatePrimeNumberArray(primeCount, flags);
        }

        private static int[] CreatePrimeNumberArray(
            int primeCount,
            NumberFlagCollection flags)
        {
            int[] primes = new int[primeCount];

            // move the primes into the result
            for (int i = 0, j = 0; i < flags.GetArraySize(); i++)
            {
                if (flags.IsPrime(i))
                    primes[j++] = i;
            }

            return primes;
        }

        private class NumberFlagCollection
        {
            private readonly bool[] _numberFlags;

            public int GetArraySize()
            {
                return _numberFlags.Length;
            }

            public bool IsPrime(int i)
            {
                return _numberFlags[i];
            }

            public NumberFlagCollection(
                int arraySize)
            {
                _numberFlags = new bool[arraySize];

                for (var i = 0; i < arraySize; i++)
                    _numberFlags[i] = true;
            }

            public void RemoveKnownNonPrimes()
            {
                _numberFlags[0] = false;
                _numberFlags[1] = false;
            }

            public void RemoveAllMultiplesOfTwo()
            {
                for (var i = 2; i < Math.Sqrt(_numberFlags.Length) + 1; i++)
                {
                    if (IsUnCrossed(i))
                        SetMultiplesAsNotPrime(i);
                }
            }

            private bool IsUnCrossed(int i)
            {
                return _numberFlags[i];
            }

            private void SetMultiplesAsNotPrime(
                int multiple)
            {
                for (var j = 2 * multiple; j < _numberFlags.Length; j += multiple)
                    _numberFlags[j] = false;
            }

            public int GetNumberOfPrimes()
            {
                int count = 0;
                for (var i = 0; i < _numberFlags.Length; i++)
                {
                    if (_numberFlags[i])
                        count++;
                }

                return count;
            }
        }
    }
}