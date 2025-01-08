using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace NewAppBookShop.Data
{
    public class DashboardRepository
    {
        private readonly string _connectionString ="Server=DESKTOP-D260V60;Database=NewAppBookShop;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True";

        public DashboardRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> GetSuccessfulOrdersCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT dbo.fn_LaySoLuongDonMuaThanhCong()";
                return await connection.ExecuteScalarAsync<int>(query);
            }
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT dbo.fn_LayTongDoanhThu()";
                return await connection.ExecuteScalarAsync<decimal>(query);
            }
        }
    }
}
