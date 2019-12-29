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

        public StockDataDto[] GetAll()
        {
            StockDataDto[] dtos = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                dtos = connection
                    .Query<StockDataDto>("SELECT * FROM [FundTradeHistory].[StockData]")
                    .ToArray();

                connection.Close();
            }

            return dtos;
        }

        public void Insert(StockDataDto[] dtos)
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

        public void DeleteById(
            StockDataId[] ids)
        {
            var sql = @"DELETE FROM
        [FundTradeHistory].[StockData]
    WHERE
        [StockId] = @StockId
        AND [Source] = @Source
        AND [DataType] = @DataType
        AND [DataDate] = @DataDate";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                connection.Execute(
                    sql,
                    ids);

                connection.Close();
            }
        }
    }
}