using System.Linq;
using Dapper;

namespace ProductionCode.FundTradeHistory
{
    public class TradeDataTableGateway
    {
        private readonly SqlConnectionHelper _sqlConnectionHelper;

        public TradeDataTableGateway(
            string connectionString)
        {
            _sqlConnectionHelper = new SqlConnectionHelper(connectionString);
        }

        public TradeDto[] GetAll()
        {
            TradeDto[] dtos = null;

            _sqlConnectionHelper.WithConnection(connection =>
            {
                dtos = connection
                    .Query<TradeDto>("SELECT * FROM [FundTradeHistory].[Trade]")
                    .ToArray();
            });

            return dtos;
        }

        private const string InsertSql = @"INSERT INTO [FundTradeHistory].[Trade]
        ([StockId]
        ,[TradeDate]
        ,[BrokerCode]
        ,[Shares])
     VALUES
        (@StockId
        ,@TradeDate
        ,@BrokerCode
        ,@Shares)";

        public int Insert(TradeDto dto)
        {
            const string sql = InsertSql + @"

SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = 0;

            _sqlConnectionHelper.WithConnection(connection =>
            {
                id = connection.Query<int>(sql, dto).Single();
            });

            return id;
        }

        public void Insert(TradeDto[] dtos)
        {
            _sqlConnectionHelper.WithConnection(connection =>
            {
                connection.Execute(InsertSql, dtos);
            });
        }

        public void DeleteById(
            int[] ids)
        {
            _sqlConnectionHelper.WithConnection(connection =>
            {
                connection.Execute(
                    "DELETE FROM [FundTradeHistory].[Trade] WHERE [TradeId] IN @TradeIds",
                    new { TradeIds = ids });
            });
        }

        public void DeleteAll()
        {
            _sqlConnectionHelper.WithConnection(connection =>
            {
                connection.Execute("DELETE FROM [FundTradeHistory].[Trade]");
            });
        }
    }
}