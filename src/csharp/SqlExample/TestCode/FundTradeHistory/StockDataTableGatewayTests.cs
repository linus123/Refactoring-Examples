using System;
using System.Linq;
using FluentAssertions;
using ProductionCode.FundTradeHistory;
using Xunit;

namespace TestCode.FundTradeHistory
{
    public class StockDataTableGatewayTests
    {
        [Fact(DisplayName = "Should insert, select, and delete records.")]
        public void Test001()
        {
            var stockDataTableGateway = new StockDataTableGateway(
                LocalDatabase.ConnectionString);

            var stockDataDto = new StockDataDto()
            {
                StockId = Guid.NewGuid(),
                Source = 1,
                DataType = 2,
                DataDate = new DateTime(2010, 1, 1),
                BrokerCode = "123",
                Value = 50.3m
            };

            stockDataTableGateway.Insert(new []{ stockDataDto });

            var stockDtos = stockDataTableGateway.GetAll();

            var target = stockDtos.FirstOrDefault(s =>
                s.StockId == stockDataDto.StockId
                && s.Source == 1
                && s.DataType == 2
                && s.DataDate.Day == stockDataDto.DataDate.Day);

            target.Should().NotBeNull();

            target.StockId.Should().Be(stockDataDto.StockId);
            target.Source.Should().Be(stockDataDto.Source);
            target.DataType.Should().Be(stockDataDto.DataType);
            target.DataDate.Should().BeCloseTo(stockDataDto.DataDate, TimeSpan.FromSeconds(2));
            target.BrokerCode.Should().Be(stockDataDto.BrokerCode);
            target.Value.Should().BeApproximately(stockDataDto.Value, 0.000001m);

            var stockDataId = new StockDataId()
            {
                StockId = stockDataDto.StockId,
                Source = stockDataDto.Source,
                DataType = stockDataDto.DataType,
                DataDate = stockDataDto.DataDate,
            };

            stockDataTableGateway.DeleteById(new[] { stockDataId,  });
        }
    }
}