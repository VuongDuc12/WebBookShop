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
    public class AuthorController : Controller
    {
        private readonly BookShopContext _context;
        public AuthorController(BookShopContext context)
        {
            _context = context;
        }
        [Route("admin/author")]
		public async Task<IActionResult> Index()
		{
			var authors = await _context.TacGia.ToListAsync();

			return View(authors);
        }

        [HttpPost]
        [Route("admin/authors/add")]
        public IActionResult Add([FromBody] TacGium newAuthor)
        {
            ModelState.Remove("Books");
            // Nếu dữ liệu không hợp lệ, trả lại lỗi
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors });
            }

            try
            {
                // Chuẩn bị câu lệnh SQL để thêm tác giả
                string sql = @"
        INSERT INTO TacGia (TenTg)
        VALUES (@TenTg)";

                // Thực thi SQL với các tham số
                var parameters = new[]
                {
            new SqlParameter("@TenTg", newAuthor.TenTg),
        };

                // Thực hiện câu lệnh SQL
                _context.Database.ExecuteSqlRaw(sql, parameters);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.Error.WriteLine(ex);

                return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm tác giả.", details = ex.Message });
            }
        }


        [Route("admin/authors/edit/{id}")]
        [HttpPut]
        public IActionResult EditAuthor(int id, [FromBody] TacGium updatedAuthor)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors });
            }

            try
            {
                // Kiểm tra xem tác giả có tồn tại hay không
                var author = _context.TacGia.FirstOrDefault(a => a.MaTg == id);
                if (author == null)
                {
                    return Json(new { success = false, message = "Tác giả không tồn tại." });
                }

                // Chuẩn bị câu lệnh SQL UPDATE
                string sql = @"
            UPDATE TacGia
            SET TenTg = @TenTg
            WHERE MaTg = @MaTg";
                Console.WriteLine($"Executing SQL: {sql}, with parameters: {updatedAuthor.TenTg}, {id}");

                // Thực thi câu lệnh SQL với tham số
                var parameters = new[]
                {
            new SqlParameter("@TenTg", updatedAuthor.TenTg),
            new SqlParameter("@MaTg", id)
        };

                // Thực thi câu lệnh SQL UPDATE
                _context.Database.ExecuteSqlRaw(sql, parameters);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.Error.WriteLine(ex);
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật tác giả.", details = ex.Message });
            }
        }


    }
}
