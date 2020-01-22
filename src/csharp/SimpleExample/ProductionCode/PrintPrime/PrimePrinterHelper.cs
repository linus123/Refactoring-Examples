using System;

namespace ProductionCode.PrintPrime
{
    public class PrimePrinterHelper
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
        private const int NumberOfPrimes = 1000;
        private const int LinesPerPage = 50;
        private const int Columns = 4;
        private const int Ordmax = 30;

        public void Invoke()
        {
            _primes = new int[NumberOfPrimes + 1];
            _multiples = new int[Ordmax + 1];

            _candidate = 1;
            _primeIndex = 1;
            _primes[1] = 2;
            _index0 = 2;
            _square = 9;

            while (_primeIndex < NumberOfPrimes)
            {
                do
                {
                    _candidate = _candidate + 2;
                    if (_candidate == _square)
                    {
                        _index0 = _index0 + 1;
                        _square = _primes[_index0] * _primes[_index0];
                        _multiples[_index0 - 1] = _candidate;
                    }
                    _i = 2;
                    _possiblyPrime = true;
                    while (_i < _index0 && _possiblyPrime)
                    {
                        while (_multiples[_i] < _candidate)
                            _multiples[_i] = _multiples[_i] + _primes[_i] + _primes[_i];
                        if (_multiples[_i] == _candidate)
                            _possiblyPrime = false;
                        _i = _i + 1;
                    }
                } while (!_possiblyPrime);
                _primeIndex = _primeIndex + 1;
                _primes[_primeIndex] = _candidate;
            }
            {
                _pageNumber = 1;
                _linesPerPage = 1;
                while (_linesPerPage <= NumberOfPrimes)
                {
                    Console.WriteLine("The First " + NumberOfPrimes +
                                       " Prime Numbers --- Page " + _pageNumber);
                    Console.WriteLine("");
                    for (_rowOffset = _linesPerPage; _rowOffset < _linesPerPage + LinesPerPage; _rowOffset++)
                    {
                        for (_column = 0; _column < Columns; _column++)
                            if (_rowOffset + _column * LinesPerPage <= NumberOfPrimes)
                                Console.Write("{0, 10}", _primes[_rowOffset + _column * LinesPerPage]);
                        Console.WriteLine("");
                    }
                    Console.WriteLine();
                    _pageNumber = _pageNumber + 1;
                    _linesPerPage = _linesPerPage + LinesPerPage * Columns;
                }
            }
        }
    }
}