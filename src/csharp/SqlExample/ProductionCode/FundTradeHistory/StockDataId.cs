using System;

namespace ProductionCode.FundTradeHistory
{
    public class StockDataId
    {
        public Guid StockId { get; set; }
        public int Source { get; set; }
        public int DataType { get; set; }
        public DateTime DataDate { get; set; }
    }
}