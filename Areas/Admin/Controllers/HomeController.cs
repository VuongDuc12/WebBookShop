using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NewAppBookShopContextConnection");
        }
[Area("Admin")]
    [Authorize]
        public async Task<IActionResult> Index()
        {
            // Kết nối tới cơ sở dữ liệu
            using (var connection = new SqlConnection(_connectionString))
            {
                // Lấy số lượng đơn hàng thành công
                var successfulOrdersCount = await connection.ExecuteScalarAsync<int>("SELECT dbo.fn_LaySoLuongDonMuaThanhCong()");
                
                // Lấy tổng doanh thu
                var totalRevenue = await connection.ExecuteScalarAsync<decimal>("SELECT dbo.fn_LayTongDoanhThu()");
                
                // Đảm bảo giá trị không null
                ViewBag.SuccessfulOrdersCount = successfulOrdersCount;
                ViewBag.TotalRevenue = totalRevenue > 0 ? totalRevenue : 0;
            }

            return View();
        }
    }
}
