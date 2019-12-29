using System;

namespace ProductionCode.FundTradeHistory
{
    public class StockDataDto
    {
        public Guid StockId { get; set; }
        public int Source { get; set; }
        public int DataType { get; set; }
        public int DataDate { get; set; }
        public string BrokerCode { get; set; }
        public decimal Value { get; set; }
    }
}