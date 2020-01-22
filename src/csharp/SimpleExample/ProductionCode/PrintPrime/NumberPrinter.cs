using System;

namespace ProductionCode.PrintPrime
{
    public class NumberPrinter
    {
        private int[] _primes;
        private int _pageNumber;
        private int _linesPerPage;
        private int _rowOffset;
        private int _column;
        private int _candidate;
        private int _primeIndex;
        private bool _possiblyPrime;
        private int _index0;
        private int _square;
        private int _i;
        private int[] _multiples;
        private const int LinesPerPage = 50;
        private const int Columns = 4;
        private const int Ordmax = 30;

        public void PrintNumbers(
            int[] primes,
            int numberOfPrimes)
        {
            _pageNumber = 1;
            _linesPerPage = 1;
            while (_linesPerPage <= numberOfPrimes)
            {
                Console.WriteLine("The First " + numberOfPrimes +
                                  " Prime Numbers --- Page " + _pageNumber);
                Console.WriteLine("");
                for (_rowOffset = _linesPerPage; _rowOffset < _linesPerPage + LinesPerPage; _rowOffset++)
                {
                    for (_column = 0; _column < Columns; _column++)
                        if (_rowOffset + _column * LinesPerPage <= numberOfPrimes)
                            Console.Write("{0, 10}", primes[_rowOffset + _column * LinesPerPage]);
                    Console.WriteLine("");
                }

                Console.WriteLine();
                _pageNumber = _pageNumber + 1;
                _linesPerPage = _linesPerPage + LinesPerPage * Columns;
            }
        }
    }
}