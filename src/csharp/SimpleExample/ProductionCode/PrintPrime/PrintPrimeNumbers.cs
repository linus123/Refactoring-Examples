using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionCode.PrintPrime
{
    class PrintPrimeNumbers
    {
        private readonly int _numberOfPrimes;
        private readonly int _linesPerPage;
        private readonly int _columns;
        private readonly int[] _primes;

        public PrintPrimeNumbers(int numberOfPrimes, int linesPerPage, int columns, int[] primes)
        {
            _numberOfPrimes = numberOfPrimes;
            _linesPerPage = linesPerPage;
            _columns = columns;
            _primes = primes;
        }

        public void Print()
        {
            int pageNumber = 1;
            int pageOffset = 1;
            while (pageOffset <= _numberOfPrimes)
            {
                PrintHeader(pageNumber);
                PrintBody(pageOffset);
                pageNumber = pageNumber + 1;
                pageOffset = pageOffset + _linesPerPage * _columns;
            }
        }

        private void PrintHeader(int pageNumber)
        {
            Console.WriteLine("The First " + _numberOfPrimes +
                              " Prime Numbers --- Page " + pageNumber);
            Console.WriteLine("");
        }

        private void PrintBody(int pageOffset)
        {
            for (int rowOffset = pageOffset; rowOffset < pageOffset + _linesPerPage; rowOffset++)
                PrintLine(rowOffset);
            
            Console.WriteLine();
        }

        private void PrintLine(int rowOffset)
        {
            for (int column = 0; column < _columns; column++)
                if (IsNotEndOfLine(rowOffset, column))
                    PrintFormattedNumber(rowOffset, column);
                

            Console.WriteLine("");
        }

        private void PrintFormattedNumber(int rowOffset, int column)
        {
            var primeIndex = rowOffset + column * _linesPerPage;
            Console.Write("{0, 10}", _primes[primeIndex]);
        }

        private bool IsNotEndOfLine(int rowOffset, int column)
        {
            return rowOffset + column * _linesPerPage <= _numberOfPrimes;
        }
    }
}
