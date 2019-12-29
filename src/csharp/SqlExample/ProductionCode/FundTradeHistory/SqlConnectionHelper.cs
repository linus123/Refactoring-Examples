using System;
using System.Data.SqlClient;

namespace ProductionCode.FundTradeHistory
{
    public class SqlConnectionHelper
    {
        private readonly string _connectionString;

        public SqlConnectionHelper(
            string connectionString)
        {
            _connectionString = connectionString;
        }

        public void WithConnection(
            Action<SqlConnection> connAction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connAction(connection);

                connection.Close();
            }
        }
    }
}