using System;

namespace ProductionCode.PrintPrime
{
    public class PrimePrinterGenerator
    {
        private const int Ordmax = 30;

        private int[] _primes;
        private int _candidate;
        private int _primeIndex;
        private int _index0;
        private int _square;
        private int[] _multiples;

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
                var possiblyPrime = true;

                do
                {
                    possiblyPrime = true;

                    _candidate = _candidate + 2;

                    if (_candidate == _square)
                    {
                        _index0 = _index0 + 1;
                        _square = _primes[_index0] * _primes[_index0];
                        _multiples[_index0 - 1] = _candidate;
                    }

                    var i = 2;

                    while (i < _index0 && possiblyPrime)
                    {
                        while (_multiples[i] < _candidate)
                            _multiples[i] = _multiples[i] + _primes[i] + _primes[i];
                        if (_multiples[i] == _candidate)
                            possiblyPrime = false;
                        i = i + 1;
                    }

                } while (!possiblyPrime);

                _primeIndex = _primeIndex + 1;
                _primes[_primeIndex] = _candidate;
            }

            return _primes;
        }
    }
}