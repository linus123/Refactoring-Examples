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

        public TradeVolumeHistory[] GetTradeVolumes(
            DateTime tradeDate,
            Guid[] stockIds)
        {
            TradeVolumeHistory[] records = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                records = connection
                    .Query<TradeVolumeHistory>(TradeVolumeQuery, new { TradeDate = tradeDate, StockIds = stockIds })
                    .ToArray();

                connection.Close();
            }

            return records;
        }

        private const string TradeVolumeQuery = @"
            SELECT [StockId], 
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
                SELECT t.[StockId], '' AS [BrokerCode], ABS(t.[Shares]) AS [Shares],
                DATEDIFF(dd, [TradeDate], @tradeDate) 
                    + CASE WHEN DATEPART(dw,  @tradeDate) = 7 THEN 1 ELSE 0 END  
                       - (DATEDIFF(wk,  [TradeDate], @tradeDate) * 2 ) 
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 END  
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 
                       END AS 'Day'   
                FROM [FundTradeHistory].[Trade] t
                WHERE t.[StockId] in @StockIds
            ) p
            PIVOT(
                SUM(p.Shares)
                FOR Day IN
                (
                    [1],[2],[3],[4],[5],[6],[7],[8],[9],[10]
                )
            ) AS pvt
        ";

        public TradeVolumeHistory[] GetMarketVolumes(
            DateTime tradeDate,
            Guid[] stockIds)
        {
            TradeVolumeHistory[] records = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                records = connection
                    .Query<TradeVolumeHistory>(MarketVolumeQuery, new { TradeDate = tradeDate, StockIds = stockIds })
                    .ToArray();

                connection.Close();
            }

            return records;
        }

        private const string MarketVolumeQuery = @"
            SELECT StockId, 
                BrokerCode, 
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
                SELECT StockId, '' AS [BrokerCode],ABS(Value) AS [Shares],
                DATEDIFF(dd, DataDate, @tradeDate) 
                    + CASE WHEN DATEPART(dw,  @tradeDate) = 7 THEN 1 ELSE 0 END 
                       - (DATEDIFF(wk,  DataDate, @tradeDate) * 2 ) 
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 END  
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 
                       END AS 'Day'   
                FROM FundTradeHistory.StockData t
                WHERE t.[StockId] in @StockIds
                    AND t.Source = 3 AND t.DataType = 4
            ) p
            PIVOT(
                SUM([Shares])
                FOR Day IN
                (
                    [1],[2],[3],[4],[5],[6],[7],[8],[9],[10]
                )
            ) AS pvt
        ";


    }
}