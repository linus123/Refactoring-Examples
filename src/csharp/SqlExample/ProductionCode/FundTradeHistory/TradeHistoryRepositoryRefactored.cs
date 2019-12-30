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
    }
}