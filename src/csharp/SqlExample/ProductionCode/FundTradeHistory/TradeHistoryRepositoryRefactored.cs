using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionCode.FundTradeHistory
{
    public class TradeHistoryRepositoryRefactored
    {
        private readonly TradeDataTableGateway _tradeDataTableGateway;

        public TradeHistoryRepositoryRefactored(
            TradeDataTableGateway tradeDataTableGateway)
        {
            _tradeDataTableGateway = tradeDataTableGateway;
        }

        public Stock[] GetTradeVolumes(
            DateTime tradeDate,
            Guid[] stockIds)
        {
            var allTrades = _tradeDataTableGateway.GetAll();

            var results = new List<Stock>();

            foreach (var stockId in stockIds)
            {
                var tradesForStock = allTrades
                    .Where(t => t.StockId == stockId)
                    .ToArray();

                var result = new Stock(
                    stockId,
                    tradeDate,
                    tradesForStock);

                results.Add(result);

            }

            return results.ToArray();
        }

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
}