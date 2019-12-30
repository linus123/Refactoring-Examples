using System;
using System.Linq;

namespace ProductionCode.FundTradeHistory
{
    public class Stock
    {
        private readonly TradeDto[] _tradeDtos;
        private readonly DateTime _tradeDate;

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
            var endDate = _tradeDate.AddDays(-1).Date;
            var startDate = _tradeDate.AddDays(-1 * i).Date;

            return _tradeDtos
                .Where(t => t.StockId == StockId && t.TradeDate.Date <= endDate && t.TradeDate.Date >= startDate)
                .Sum(d => Math.Abs(d.Shares));
        }
    }
}