using System;
using System.Linq;

namespace ProductionCode.FundTradeHistory
{
    public class Stock
    {
        private readonly TradeDto[] _tradeDtos;
        private readonly DateTime _tradeDate;
        private DateTime[] _startDates;

        public Stock(
            Guid stockId,
            DateTime tradeDate,
            TradeDto[] tradeDtos)
        {
            StockId = stockId;
            _tradeDate = tradeDate;
            _tradeDtos = tradeDtos;
        }

        public Guid StockId { get; private set; }

        public decimal GetAccumulatedDayVolume(int i)
        {
            if (_startDates == null)
                _startDates = CalculateStartDates(_tradeDate);

            var endDate = _tradeDate.AddDays(-1).Date;
            var startDate = _startDates[i - 1];

            return _tradeDtos
                .Where(t =>
                {
                    return t.StockId == StockId
                        && IsBetween(t.TradeDate, startDate, endDate);
                })
                .Sum(d => Math.Abs(d.Shares));
        }

        private bool IsBetween(
            DateTime dateUnderTest,
            DateTime startDate,
            DateTime endDate)
        {
            return dateUnderTest.Date <= endDate && dateUnderTest.Date >= startDate;
        }

        private static DateTime[] CalculateStartDates(
            DateTime tradeDate)
        {
            var startDate = new DateTime[10];

            var daysToSubtract = -1;

            for (int dayCounter = 0; dayCounter < 10; dayCounter++)
            {
                var startDateUnderEval = tradeDate.AddDays(daysToSubtract - dayCounter);
                
                if (startDateUnderEval.DayOfWeek == DayOfWeek.Monday)
                {
                    // Include in previous Sunday's trades.
                    daysToSubtract--;
                    startDateUnderEval = tradeDate.AddDays(daysToSubtract - dayCounter);
                }

                if (startDateUnderEval.DayOfWeek == DayOfWeek.Saturday)
                {
                    // Include in Friday's trades.
                    daysToSubtract--;
                    startDateUnderEval = tradeDate.AddDays(daysToSubtract - dayCounter);
                }

                startDate[dayCounter] = startDateUnderEval;
            }

            return startDate;
        }
    }
}