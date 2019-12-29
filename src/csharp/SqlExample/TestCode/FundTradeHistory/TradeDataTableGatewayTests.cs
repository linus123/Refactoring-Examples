using System;
using System.Linq;
using FluentAssertions;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class TradeDataTableGatewayTests
    {
        [Fact(DisplayName= "Should insert, select, and delete records.")]
        public void Test001()
        {
            var tradeDataTableGateway = new TradeDataTableGateway(
                LocalDatabase.ConnectionString);

            var tradeDto = new TradeDto()
            {
                StockId = Guid.NewGuid(),
                TradeDate = new DateTime(2010, 1, 1),
                BrokerCode = "123",
                Shares = 200.3m
            };

            var tradeId = tradeDataTableGateway.Insert(tradeDto);

            var tradeDtos = tradeDataTableGateway.GetAll();

            var target = tradeDtos.FirstOrDefault(t => t.TradeId == tradeId);

            target.Should().NotBeNull();

            target.TradeId.Should().Be(tradeId);
            target.StockId.Should().Be(tradeDto.StockId);
            target.TradeDate.Should().BeCloseTo(tradeDto.TradeDate, TimeSpan.FromSeconds(2));
            target.BrokerCode.Should().Be(tradeDto.BrokerCode);
            target.Shares.Should().BeApproximately(tradeDto.Shares, 0.000001m);

            tradeDataTableGateway.DeleteById(new []{ tradeId });
        }
    }
}