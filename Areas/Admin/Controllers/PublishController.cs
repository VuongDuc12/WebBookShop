using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Data;
using NewAppBookShop.Models;

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublishController : Controller
    {
        private readonly BookShopContext _context;
        
        public PublishController(BookShopContext context)
        {
            _context = context;
        }
        [Route("admin/publishs")]
        public async Task<IActionResult> Index()
        {
            var publishs = await _context.NhaXuatBans.ToListAsync();

            return View(publishs);
        }


        [HttpPost]
        [Route("admin/publishs/add")]
        public IActionResult Add([FromBody] NhaXuatBan newPublisher)
        {
            ModelState.Remove("Saches"); // Xoá kiểm tra trường Saches vì nó là navigation property

            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors });
            }

            try
            {
                // Câu lệnh SQL để thêm nhà xuất bản
                string sql = @"
        INSERT INTO NhaXuatBan (TenNxb, SoDienThoai, DiaChi)
        VALUES (@TenNxb, @SoDienThoai, @DiaChi)";

                // Thực thi SQL với tham số
                var parameters = new[]
                {
            new SqlParameter("@TenNxb", newPublisher.TenNxb),
            new SqlParameter("@SoDienThoai", newPublisher.SoDienThoai ?? (object)DBNull.Value),
            new SqlParameter("@DiaChi", newPublisher.DiaChi ?? (object)DBNull.Value)
        };

                _context.Database.ExecuteSqlRaw(sql, parameters);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.Error.WriteLine(ex);

                return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm nhà xuất bản.", details = ex.Message });
            }
        }


        [Route("admin/publishs/edit/{id}")]
        [HttpPut]
        public IActionResult EditPublisher(int id, [FromBody] NhaXuatBan updatedPublisher)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors });
            }

            try
            {
                // Kiểm tra xem nhà xuất bản có tồn tại không
                var publisher = _context.NhaXuatBans.FirstOrDefault(p => p.MaNxb == id);
                if (publisher == null)
                {
                    return Json(new { success = false, message = "Nhà xuất bản không tồn tại." });
                }

                // Cập nhật thông tin nhà xuất bản
                publisher.TenNxb = updatedPublisher.TenNxb;
                publisher.SoDienThoai = updatedPublisher.SoDienThoai;
                publisher.DiaChi = updatedPublisher.DiaChi;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.Error.WriteLine(ex);

                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật nhà xuất bản.", details = ex.Message });
            }
        }
        
    }

    // Model để chứa kết quả trả về
   


}
