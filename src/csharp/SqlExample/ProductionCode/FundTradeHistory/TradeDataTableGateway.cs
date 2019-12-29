using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ProductionCode.FundTradeHistory
{
    public class TradeDataTableGateway
    {
        private readonly string _connectionString;

        public TradeDataTableGateway(
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
                    .Query<TradeDto>("SELECT * FROM [FundTradeHistory].[Trade]")
                    .ToArray();

                connection.Close();
            }

            return dtos;
        }

        public void Insert(TradeDto[] dtos)
        {
            const string sql = @"INSERT INTO [FundTradeHistory].[Trade]
        ([TradeId]
        ,[StockId]
        ,[TradeDate]
        ,[BrokerCode]
        ,[Shares])
     VALUES
        (@TradeId
        ,@StockId
        ,@TradeDate
        ,@BrokerCode
        ,@Shares)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(sql, dtos);

                connection.Close();
            }
        }
    }
}