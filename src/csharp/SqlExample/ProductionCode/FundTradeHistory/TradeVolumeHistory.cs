﻿using System;

namespace ProductionCode.FundTradeHistory
{
    public class TradeVolumeHistory
    {
        private int length = 10;

        public Guid StockId { get; set; }
        public string BrokerCode { get; set; }
        public decimal Day1 { get; set; }
        public decimal Day2 { get; set; }
        public decimal Day3 { get; set; }
        public decimal Day4 { get; set; }
        public decimal Day5 { get; set; }
        public decimal Day6 { get; set; }
        public decimal Day7 { get; set; }
        public decimal Day8 { get; set; }
        public decimal Day9 { get; set; }
        public decimal Day10 { get; set; }

        private decimal[] _accumulated10DayVolume;

        public void Accumulate10DayVolume()
        {
            _accumulated10DayVolume = new decimal[length];
            for (int i = 0; i < length; i++)
            {
                _accumulated10DayVolume[i] = GetDayVolume(i + 1);
            }
            for (int i = 1; i < length; i++)
            {
                _accumulated10DayVolume[i] += _accumulated10DayVolume[i - 1];
            }
        }

        public decimal GetAccumulatedDayVolume(int dayNo)
        {
            decimal accumulatedVolume = 0;
            if (_accumulated10DayVolume != null)
            {
                dayNo -= 1;
                if (dayNo >= 0 && dayNo < _accumulated10DayVolume.Length)
                {
                    accumulatedVolume = _accumulated10DayVolume[dayNo];
                }
            }
            return accumulatedVolume;
        }

        private decimal GetDayVolume(int dayNo)
        {
            decimal dayVolume = 0;
            switch (dayNo)
            {
                case 1:
                    dayVolume = Day1;
                    break;
                case 2:
                    dayVolume = Day2;
                    break;
                case 3:
                    dayVolume = Day3;
                    break;
                case 4:
                    dayVolume = Day4;
                    break;
                case 5:
                    dayVolume = Day5;
                    break;
                case 6:
                    dayVolume = Day6;
                    break;
                case 7:
                    dayVolume = Day7;
                    break;
                case 8:
                    dayVolume = Day8;
                    break;
                case 9:
                    dayVolume = Day9;
                    break;
                case 10:
                    dayVolume = Day10;
                    break;
                default:
                    break;
            }
            return dayVolume;
        }
    }

}