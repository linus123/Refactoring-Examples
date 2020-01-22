using System;

namespace ProductionCode.PrintPrime
{
    public class PrintPrimes
    {
        private const int NumberOfPrimes = 1000;

        public void Run()
        {
            var primePrinterHelper = new PrimePrinterGenerator();

            var primes = primePrinterHelper.GeneratePrimes(NumberOfPrimes);

            var numberPrinter = new NumberPrinter();

            numberPrinter.PrintNumbers(primes, NumberOfPrimes);
        }
    }
}