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
        private const int LinesPerPage = 50;
        private const int Columns = 4;
        private const int Ordmax = 30;

        public int[] GeneratePrimes(
            int numberOfPrimes)
        {
            _primes = new int[numberOfPrimes + 1];
            _multiples = new int[Ordmax + 1];

            _candidate = 1;
            _primeIndex = 1;
            _primes[1] = 2;
            _index0 = 2;
            _square = 9;

            while (_primeIndex < numberOfPrimes)
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

            return _primes;
        }
    }
}