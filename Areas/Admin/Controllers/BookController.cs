using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Data;
using NewAppBookShop.Models;

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly BookShopContext _context;

        public BookController(BookShopContext context)
        {
            _context = context;
        }

        [Route("admin/books")]
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả sách, bao gồm thông tin về tác giả và nhà xuất bản
            var books = await _context.Saches
                .Include(b => b.MaTacGiaNavigation)  // Liên kết với tác giả
                .Include(b => b.MaNxbNavigation)    // Liên kết với nhà xuất bản
                .Include(b => b.MaTheLoaiNavigation)
                .ToListAsync();

            var authors = await _context.TacGia.ToListAsync();
            var publishers = await _context.NhaXuatBans.ToListAsync();
            var categorys = await _context.TheLoais.ToListAsync();
            // Truyền danh sách sách, tác giả và nhà xuất bản vào view
            ViewBag.Authors = authors;
            ViewBag.Publishers = publishers;
            ViewBag.Category = categorys;
            // Truyền danh sách sách vào view
            return View(books);
        }

        [HttpDelete]
        [Route("admin/books/delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var book = await _context.Saches.FindAsync(id);
                if (book == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy sách cần xóa." });
                }

                // Thực hiện xóa sách bằng SQL trực tiếp để tránh trigger gây lỗi
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Sach WHERE MaSach = {0}", id);

                return Json(new { success = true, message = "Thay đổi trạng thái sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("admin/books/add")]
        public IActionResult Add([FromBody] Sach newBook)
        {
            if (!ModelState.IsValid)
            {
                // In ra các lỗi chi tiết
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors });
            }

            try
            {
                // Thêm sách vào cơ sở dữ liệu
                _context.Saches.Add(newBook);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
