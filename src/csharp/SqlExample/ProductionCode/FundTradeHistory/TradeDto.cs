using System;

namespace ProductionCode.FundTradeHistory
{
    public class TradeDto
    {
        public int TradeId { get; set; }
        public Guid StockId { get; set; }
        public DateTime TradeDate { get; set; }
        public string BrokerCode { get; set; }
        public decimal Shares { get; set; }
    }
}