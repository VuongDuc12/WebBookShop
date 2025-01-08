using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using NewAppBookShop.Areas.Identity.Data;
using NewAppBookShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NewAppBookShop.ViewModels;

public class CartController : Controller
{
    private readonly string _connectionString = "Server=DESKTOP-D260V60;Database=NewAppBookShop;Trusted_Connection=True;Encrypt=False;";
    private readonly UserManager<APpUser> _userManager;

    public CartController(UserManager<APpUser> userManager)
    {
        _userManager = userManager;
    }

    // Hiển thị giỏ hàng
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "Bạn cần đăng nhập để xem giỏ hàng.";
            return RedirectToAction("Login", "Account");
        }

        List<ChiTietGioHangViewModel> cartItems;
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                // Get cart ID using stored procedure sp_LayIdGioHang
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.Id, DbType.String);
                parameters.Add("@GioHangId", 0, DbType.Int64, ParameterDirection.Output);
                
                // Execute stored procedure to get or create the cart
                await connection.ExecuteAsync("sp_LayIdGioHang", parameters, commandType: CommandType.StoredProcedure);
                
                // Retrieve the output parameter (cartId)
                var cartId = parameters.Get<long>("@GioHangId");

                if (cartId == 0)
                {
                    return View(new List<ChiTietGioHangViewModel>());
                }

                // Get the products in the cart
                cartItems = (await connection.QueryAsync<ChiTietGioHangViewModel>(@"
                    SELECT ct.MaSach, b.TenSach, ct.SoLuong, b.GiaBan, (ct.SoLuong * b.GiaBan) AS ThanhTien
                    FROM ChiTietGioHang ct
                    JOIN Sach b ON ct.MaSach = b.MaSach
                    WHERE ct.GioHangId = @GioHangId",
                    new { GioHangId = cartId })
                ).ToList();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể kết nối đến cơ sở dữ liệu.";
                // Log exception for further debugging
                return View(new List<ChiTietGioHangViewModel>());
            }
        }

        return View(cartItems);
    }

    // Thêm sản phẩm vào giỏ hàng
    [HttpPost]
    public async Task<IActionResult> AddToCart(int bookId, int quantity, decimal price)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "Bạn cần đăng nhập để thêm sản phẩm.";
            return RedirectToAction("Login", "Account");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                // Get or create the cart
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.Id, DbType.String);
                parameters.Add("@GioHangId", 0, DbType.Int64, ParameterDirection.Output);
                
                // Execute stored procedure to get the cart ID
                await connection.ExecuteAsync("sp_LayIdGioHang", parameters, commandType: CommandType.StoredProcedure);
                
                // Retrieve the cart ID
                var cartId = parameters.Get<long>("@GioHangId");

                // Add product to the cart
                var addItemParams = new
                {
                    GioHangId = cartId,
                    MaSach = bookId,
                    SoLuong = quantity,
                    
                };

                // Execute the stored procedure to add the product to the cart
                await connection.ExecuteAsync("sp_ThemSanPhamVaoGioHang", addItemParams, commandType: CommandType.StoredProcedure);

                TempData["Success"] = "Sản phẩm đã được thêm vào giỏ hàng.";
            }
            catch (Exception ex)
            {
                // If there is an error during execution, save the error to TempData
                TempData["Error"] = "Không thể thêm sản phẩm vào giỏ hàng. Lỗi: " + ex.Message;
            }
        }

        // After adding the product, return to the home page or product page
        return RedirectToAction("Index", "Home");
    }


   [HttpPost]
public async Task<IActionResult> Checkout()
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
    {
        TempData["Error"] = "Bạn cần đăng nhập để thanh toán.";
        return RedirectToAction("Login", "Account");
    }

    // Lấy MaKh từ quan hệ KhachHangs

  

    using (var connection = new SqlConnection(_connectionString))
    {
        try
        {
            connection.Open();
            var query = @"
                SELECT MaKh
                FROM KhachHang
                WHERE UserId = @UserId"; // Giả định UserId là cột liên kết

            // Thực thi truy vấn
            var customerId = await connection.QueryFirstOrDefaultAsync<long?>(query, new { UserId = user.Id });
            // Lấy ID giỏ hàng
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", user.Id, DbType.String);
            parameters.Add("@GioHangId", 0, DbType.Int64, ParameterDirection.Output);

            await connection.ExecuteAsync("sp_LayIdGioHang", parameters, commandType: CommandType.StoredProcedure);
            var cartId = parameters.Get<long>("@GioHangId");

            if (cartId == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn trống.";
                return RedirectToAction("Index");
            }

            // Thanh toán giỏ hàng
            var checkoutParams = new DynamicParameters();
            checkoutParams.Add("@GioHangId", cartId, DbType.Int64);
            checkoutParams.Add("@MaKh", customerId, DbType.Int64); // Sử dụng MaKh từ KhachHang
            checkoutParams.Add("@MaNv", DBNull.Value, DbType.Int64); // Nếu không có nhân viên, để null
            checkoutParams.Add("@SoHdmua", 0, DbType.Int64, ParameterDirection.Output);

            await connection.ExecuteAsync("sp_ThanhToanGioHang", checkoutParams, commandType: CommandType.StoredProcedure);

            // Lấy số hóa đơn mua mới tạo
            var orderId = checkoutParams.Get<long>("@SoHdmua");

            TempData["Success"] = $"Thanh toán thành công! Số hóa đơn của bạn là {orderId}.";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi trong quá trình thanh toán: {ex.Message}";
            return RedirectToAction("Index" ,"Home");
        }
    }
}

}
