using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ProductionCode.TradeVolume
{
    public class TradeHistoryRepository
    {
        private readonly string _connectionString;

        public TradeHistoryRepository(
            string connectionString)
        {
            _connectionString = connectionString;
        }

        public TradeVolumeHistory[] GetTradeVolumeHistories()
        {
            TradeVolumeHistory[] tradeVolumeHistories = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                tradeVolumeHistories = connection
                    .Query<TradeVolumeHistory>(TradeVolumesQuery)
                    .ToArray();

                connection.Close();
            }

            return tradeVolumeHistories;
        }

        private const string TradeVolumesQuery = @"
            CREATE TABLE #CusipSedol
            (
                CusipSedol VARCHAR(30) NOT NULL,
                PRIMARY KEY(CusipSedol)
            )

            INSERT INTO #CusipSedol 
            SELECT pairs.FirstValue as 'CusipSedol' 
            FROM dbo.udf_ParseValuePairsToTable(@cusipSedol,',',';') as pairs

            SELECT CusipSedol, 
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
                SELECT Cusip_Sedol AS 'CusipSedol', Broker_Code AS 'BrokerCode',ABS(Trade_Shares) AS 'Trade_Shares',
                DATEDIFF(dd, Trade_Date, @tradeDate) 
                    + CASE WHEN DATEPART(dw,  @tradeDate) = 7 THEN 1 ELSE 0 END  
                       - (DATEDIFF(wk,  Trade_Date, @tradeDate) * 2 ) 
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 END  
                       - CASE WHEN DATEPART(dw,  @tradeDate) = 1 THEN 1 ELSE 0 
                       END AS 'Day'   
                FROM #CusipSedol s JOIN dbo.tblTrading_Trades t ON s.CusipSedol = t.Cusip_Sedol
            ) p
            PIVOT(
                SUM(Trade_Shares)
                FOR Day IN
                (
                    [1],[2],[3],[4],[5],[6],[7],[8],[9],[10]
                )
            ) AS pvt
        ";
    }
}