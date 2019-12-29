using System;
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

        public Result[] GetTradeVolumes(
            DateTime tradeDate,
            Guid[] stockIds)
        {
            var guids = _tradeDataTableGateway.GetStockIds();

            return guids.Select(g => new Result() {StockId = g}).ToArray();
        }

        public class Result
        {
            public Guid StockId { get; set; }

            public decimal GetAccumulatedDayVolume(int i)
            {
                return 0;
            }
        }
    }
}