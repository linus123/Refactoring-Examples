using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ProductionCode.FundTradeHistory
{
    public class StockDataTableGateway
    {
        private readonly string _connectionString;

        public StockDataTableGateway(
            string connectionString)
        {
            _connectionString = connectionString;
        }

        public TradeDto[] GetAll()
        {
            TradeDto[] dtos = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                dtos = connection
                    .Query<TradeDto>("SELECT * FROM [FundTradeHistory].[StockData]")
                    .ToArray();

                connection.Close();
            }

            return dtos;
        }

        public void Insert(TradeDto[] dtos)
        {
            const string sql = @"INSERT INTO [FundTradeHistory].[StockData]
        ([StockId]
        ,[Source]
        ,[DataType]
        ,[DataDate]
        ,[BrokerCode]
        ,[Value])
    VALUES
        (@StockId
        ,@Source
        ,@DataType
        ,@DataDate
        ,@BrokerCode
        ,@Value)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(sql, dtos);

                connection.Close();
            }
        }
    }
}