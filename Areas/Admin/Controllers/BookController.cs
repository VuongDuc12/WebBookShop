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
            try
            {
                // Lấy tất cả sách, bao gồm thông tin về tác giả, nhà xuất bản, thể loại
                var books = await _context.Saches
                    .Include(b => b.MaTacGiaNavigation)  // Liên kết với tác giả
                    .Include(b => b.MaNxbNavigation)    // Liên kết với nhà xuất bản
                    .Include(b => b.MaTheLoaiNavigation)
                    .ToListAsync();

                // Kiểm tra và xử lý các trường có thể null
                foreach (var book in books)
                {
                    // Kiểm tra trường Anh có null không, nếu có thì gán giá trị mặc định
                    if (string.IsNullOrEmpty(book.Anh))
                    {
                        book.Anh = "/images/default-book-image.jpg"; // Hình ảnh mặc định nếu không có ảnh
                    }

                    // Kiểm tra nếu có bất kỳ liên kết nào null, bạn có thể xử lý như gán một giá trị mặc định
                    if (book.MaTacGiaNavigation == null)
                    {
                        book.MaTacGiaNavigation = new TacGium { TenTg = "Chưa có tác giả" };
                    }

                    if (book.MaNxbNavigation == null)
                    {
                        book.MaNxbNavigation = new NhaXuatBan { TenNxb = "Chưa có nhà xuất bản" };
                    }

                    if (book.MaTheLoaiNavigation == null)
                    {
                        book.MaTheLoaiNavigation = new TheLoai { TenTheLoai = "Chưa có thể loại" };
                    }
                    if (book.Anh == null)
                    {
                        book.Anh =  "Chưa có thể loại";
                    }
                }

                // Lấy danh sách tác giả, nhà xuất bản, thể loại
                var authors = await _context.TacGia.ToListAsync();
                var publishers = await _context.NhaXuatBans.ToListAsync();
                var categories = await _context.TheLoais.ToListAsync();

                // Truyền các dữ liệu vào ViewBag
                ViewBag.Authors = authors;
                ViewBag.Publishers = publishers;
                ViewBag.Category = categories;

                // Truyền danh sách sách vào view
                return View(books);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.Error.WriteLine(ex);
                return Json(new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu.", details = ex.Message });
            }
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
                    return NotFound(new { success = false, message = "Không tìm thấy sách." });
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

        [HttpPut]
        [Route("admin/books/Pushlish/{id}")]
        public async Task<IActionResult> Pushlish(long id)
        {
            try
            {
                var book = await _context.Saches.FindAsync(id);
                if (book == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy sách ." });
                }

                // Thực hiện xóa sách bằng SQL trực tiếp để tránh trigger gây lỗi
                await _context.Database.ExecuteSqlRawAsync("Update Sach set TrangThai = 1 WHERE MaSach = {0}", id);

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
            ModelState.Remove("MaNxbNavigation");
            ModelState.Remove("MaTacGiaNavigation");
            ModelState.Remove("MaTheLoaiNavigation");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors });
            }

            try
            {
                // Chuẩn bị câu lệnh SQL
                string sql = @"
        INSERT INTO Sach (TenSach, MaTacGia, MaNxb, MaTheLoai, GiaBan, TrangThai)
        VALUES (@TenSach, @MaTacGia, @MaNxb, @MaTheLoai, @GiaBan, @TrangThai)";

                // Thực thi SQL với các tham số
                var parameters = new[]
                {
            new SqlParameter("@TenSach", newBook.TenSach),
            new SqlParameter("@MaTacGia", newBook.MaTacGia),
            new SqlParameter("@MaNxb", newBook.MaNxb),
            new SqlParameter("@MaTheLoai", newBook.MaTheLoai),
            new SqlParameter("@GiaBan", newBook.GiaBan),
            new SqlParameter("@TrangThai", newBook.TrangThai)
        };

                _context.Database.ExecuteSqlRaw(sql, parameters);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần (để tiện debug)
                Console.Error.WriteLine(ex);

                return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm sách.", details = ex.Message });
            }
        }
        [HttpGet("/admin/books/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _context.Saches.FirstOrDefault(b => b.MaSach == id);
            if (book == null)
            {
                return Json(new { success = false, message = "Sách không tồn tại!" });
            }

            return Json(book);
        }

        // Cập nhật thông tin sách
        [HttpPut("/admin/books/update/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Sach updatedBook)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            var book = _context.Saches.FirstOrDefault(b => b.MaSach == id);
            if (book == null)
            {
                return Json(new { success = false, message = "Sách không tồn tại!" });
            }

            // Cập nhật thông tin sách
            book.TenSach = updatedBook.TenSach;
            book.MaTacGia = updatedBook.MaTacGia;
            book.MaNxb = updatedBook.MaNxb;
            book.MaTheLoai = updatedBook.MaTheLoai;
            book.GiaBan = updatedBook.GiaBan;
            book.Anh = updatedBook.Anh;

            _context.SaveChanges();

            return Json(new { success = true, message = "Cập nhật sách thành công!" });
        }
    }



}

