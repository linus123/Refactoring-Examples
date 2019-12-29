using System;
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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                id = connection.Query<int>(sql, dto).Single();

                connection.Close();
            }

            return id;
        }

        public void Insert(TradeDto[] dtos)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(InsertSql, dtos);

                connection.Close();
            }
        }

        public void DeleteById(
            int[] ids)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(
                    "DELETE FROM [FundTradeHistory].[Trade] WHERE [TradeId] IN @TradeIds",
                    new { TradeIds = ids });

                connection.Close();
            }
        }
    }
}