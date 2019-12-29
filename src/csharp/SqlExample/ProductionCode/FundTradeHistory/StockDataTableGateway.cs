using System.Linq;
using Dapper;

namespace ProductionCode.FundTradeHistory
{
    public class StockDataTableGateway
    {
        private readonly SqlConnectionHelper _sqlConnectionHelper;

        public StockDataTableGateway(
            string connectionString)
        {
            _sqlConnectionHelper = new SqlConnectionHelper(connectionString);
        }

        public StockDataDto[] GetAll()
        {
            StockDataDto[] dtos = null;

            _sqlConnectionHelper.WithConnection(connection =>
            {
                dtos = connection
                    .Query<StockDataDto>("SELECT * FROM [FundTradeHistory].[StockData]")
                    .ToArray();
            });

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

            _sqlConnectionHelper.WithConnection(connection =>
            {
                connection.Execute(sql, dtos);
            });
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

            _sqlConnectionHelper.WithConnection(connection =>
            {
                connection.Execute(sql, ids);
            });
        }

        public void DeleteAll()
        {
            _sqlConnectionHelper.WithConnection(connection =>
            {
                connection.Execute("DELETE FROM [FundTradeHistory].[StockData]");
            });
        }

    }
}