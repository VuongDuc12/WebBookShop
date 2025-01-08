using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Data;
using NewAppBookShop.Models;
using Newtonsoft.Json;

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ImportBillController : Controller
    {
        private readonly BookShopContext _context;

        public ImportBillController(BookShopContext context)
        {
            _context = context;
        }
        [Route("admin/importbill/list")]

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        [Route("admin/importbill/create")]
        public IActionResult Create()
        {
            // Lấy danh sách sách từ cơ sở dữ liệu
            var books = _context.Saches.ToList();
            ViewBag.Books = books; // Đưa danh sách sách vào ViewBag để sử dụng trong view
            return View();
        }

        [HttpPost]
        [Route("admin/importbill/add")]
        public async Task<IActionResult> Add()
        {
            if (ModelState.IsValid)
            {
                // Chuỗi kết nối đến database
                string connectionString = "Server=HOANGLANNN\\SQLEXPRESS;Database=NewAppBookShop;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Đọc và phân tích JSON từ body
                            string jsonData;
                            using (var reader = new StreamReader(Request.Body))
                            {
                                jsonData = await reader.ReadToEndAsync();
                            }

                            // Chuyển đổi JSON thành danh sách chi tiết hóa đơn nhập
                            var chiTietHoaDonNhaps = JsonConvert.DeserializeObject<List<ChiTietHoaDonNhap>>(jsonData);

                            if (chiTietHoaDonNhaps == null || !chiTietHoaDonNhaps.Any())
                            {
                                return Json(new { Success = false, Message = "Dữ liệu không hợp lệ hoặc trống!" });
                            }

                            // Bước 1: Tạo hóa đơn nhập với giá trị mặc định
                            string insertHoaDonNhapQuery = @"
                    INSERT INTO HoaDonNhap (MaNV, TongTien, TrangThai)
                    OUTPUT INSERTED.SoHDNhap
                    VALUES (2, 0, 'HOÀN THÀNH')";

                            long soHDNhapMoi;
                            using (var cmd = new SqlCommand(insertHoaDonNhapQuery, connection, transaction))
                            {
                                soHDNhapMoi = (long)await cmd.ExecuteScalarAsync();
                            }

                            // Bước 2: Thêm chi tiết hóa đơn nhập
                            string insertChiTietQuery = @"
                    INSERT INTO ChiTietHoaDonNhap (SoHDNhap, MaSach, SoLuong, GiaNhap)
                    VALUES (@SoHDNhap, @MaSach, @SoLuong, @GiaNhap)";

                            decimal tongTien = 0;

                            foreach (var ct in chiTietHoaDonNhaps)
                            {
                                using (var cmd = new SqlCommand(insertChiTietQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhapMoi);
                                    cmd.Parameters.AddWithValue("@MaSach", ct.MaSach);
                                    cmd.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
                                    cmd.Parameters.AddWithValue("@GiaNhap", ct.GiaNhap);

                                    tongTien += ct.SoLuong * ct.GiaNhap;

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }

                            // Bước 3: Cập nhật tổng tiền trong bảng hóa đơn nhập
                            string updateHoaDonNhapQuery = @"
                    UPDATE HoaDonNhap
                    SET TongTien = @TongTien
                    WHERE SoHDNhap = @SoHDNhap";

                            using (var cmd = new SqlCommand(updateHoaDonNhapQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                                cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhapMoi);

                                await cmd.ExecuteNonQueryAsync();
                            }

                            // Commit transaction
                            await transaction.CommitAsync();

                            // Trả về kết quả
                            return Json(new { Success = true, Message = "Hóa đơn nhập đã được tạo thành công!" });
                        }
                        catch (Exception ex)
                        {
                            // Rollback nếu có lỗi
                            await transaction.RollbackAsync();
                            return Json(new { Success = false, Message = "Đã xảy ra lỗi: " + ex.Message });
                        }
                    }
                }
            }

            // Nếu có lỗi, trả về lỗi
            return Json(new { Success = false, Message = "Dữ liệu không hợp lệ!" });
        }



    }
}
