using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionCode.PrintPrime
{
    public class NumberPrinter
    {
        private const int LinesPerPage = 50;
        private const int Columns = 4;

        private int[] _primes;
        private int _numberOfPrimes;

        public void PrintNumbers(
            int[] primes,
            int numberOfPrimes)
        {
            _numberOfPrimes = numberOfPrimes;
            _primes = primes;

            var pageNumber = 1;
            var pageOffset = 1;

            while (pageOffset <= numberOfPrimes)
            {
                WritePageHeader(pageNumber);
                WritePageBody(pageOffset);
                WritePageFooter();

                pageNumber = pageNumber + 1;
                pageOffset = pageOffset + LinesPerPage * Columns;
            }
        }

        private void WritePageHeader(int pageNumber)
        {
            Console.WriteLine(CreateHeaderLine(pageNumber));
            Console.WriteLine();
        }

        private void WritePageBody(int pageOffset)
        {
            var lines = GetNumberLines(pageOffset);

            foreach (var line in lines)
                Console.WriteLine(line);
        }

        private static void WritePageFooter()
        {
            Console.WriteLine();
        }

        private IEnumerable<string> GetNumberLines(
            int pageOffset)
        {
            for (var rowOffset = pageOffset; rowOffset < pageOffset + LinesPerPage; rowOffset++)
                yield return CreateSingleLine(rowOffset);
        }

        private string CreateSingleLine(
            int rowOffset)
        {
            var line = new StringBuilder();

            for (var column = 0; column < Columns; column++)
            {
                var primeIndex = rowOffset + column * LinesPerPage;

                if (primeIndex <= _numberOfPrimes)
                    line.Append(CreateRightAlignedNumber(_primes[primeIndex]));
            }

            return line.ToString();
        }

        private string CreateRightAlignedNumber(
            int number)
        {
            return $"{number, 10}";
        }

        private string CreateHeaderLine(int pageNumber)
        {
            return "The First "
                   + _numberOfPrimes
                   +  " Prime Numbers --- Page "
                   + pageNumber;
        }
    }
}