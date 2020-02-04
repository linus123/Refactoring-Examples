using System;
using System.Runtime.CompilerServices;

namespace ProductionCode.PrintPrime
{
    public class PrintPrimes
    {
        public void Run()
        {
            const int numberOfPrimes = 1000;
            const int linesPerPage = 50;
            const int columns = 4;

            var calculatePrime = new CalculatePrimes();
            var primes = calculatePrime.CalculatePrime(numberOfPrimes);

            var printPrimeNumbers = new PrintPrimeNumbers(numberOfPrimes, linesPerPage, columns, primes);
            printPrimeNumbers.Print();
        }
    }
}