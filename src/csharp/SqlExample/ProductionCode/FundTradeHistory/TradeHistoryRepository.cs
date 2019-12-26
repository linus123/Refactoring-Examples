using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ProductionCode.FundTradeHistory
{
    public class TradeHistoryRepository
    {
        private readonly string _connectionString;

        public TradeHistoryRepository(
            string connectionString)
        {
            _connectionString = connectionString;
        }

        public TradeVolumeHistory[] GetTradeVolumeHistories(
            DateTime tradeDate,
            Guid[] fundIds)
        {
            TradeVolumeHistory[] tradeVolumeHistories = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                tradeVolumeHistories = connection
                    .Query<TradeVolumeHistory>(TradeVolumesQuery, new { TradeDate = tradeDate, FundIds = fundIds })
                    .ToArray();

                connection.Close();
            }

            return tradeVolumeHistories;
        }

        private const string TradeVolumesQuery = @"
            SELECT [FundId], 
                [BrokerCode], 
                ISNULL([1],0) AS Day1,
                ISNULL([2],0) AS Day2,
                ISNULL([3],0) AS Day3,
                ISNULL([4],0) AS Day4,
                ISNULL([5],0) AS Day5,
                ISNULL([6],0) AS Day6,
                ISNULL([7],0) AS Day7,
                ISNULL([8],0) AS Day8,
                ISNULL([9],0) AS Day9,
                ISNULL([10],0) AS Day10
            FROM
            (
                SELECT t.[FundId], t.[BrokerCode], ABS(t.[Shares]) AS 'Shares',
                DATEDIFF(dd, [TradeDate], @tradeDate) 
                    + CASE WHEN DATEPART(dw,  @tradeDate) = 7 THEN 1 ELSE 0 END  
                       - (DATEDIFF(wk,  [TradeDate], @tradeDate) * 2 ) 
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 END  
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 
                       END AS 'Day'   
                FROM [FundTradeHistory].[Trade] t
                WHERE t.[FundId] in @FundIds
            ) p
            PIVOT(
                SUM(p.Shares)
                FOR Day IN
                (
                    [1],[2],[3],[4],[5],[6],[7],[8],[9],[10]
                )
            ) AS pvt
        ";
    }
}