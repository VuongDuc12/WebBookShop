using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Data;
using NewAppBookShop.Models;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using NewAppBookShop.Areas.Admin.ViewModel;

namespace NewAppBookShop.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookShopContext _context;
        string connectionString = "Server=DESKTOP-D260V60;Database=NewAppBookShop;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;";

        public HomeController(ILogger<HomeController> logger, BookShopContext context)
        {
            _logger = logger;
            _context = context;

        }

		public async Task<IActionResult> Index(int page = 1)
		{
			var books = new List<NewAppBookShop.ViewModels.BookViewModel>();
			string connectionString = "Server=DESKTOP-D260V60;Database=NewAppBookShop;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;";

			int pageSize = 4; // Số sách mỗi trang
			int skip = (page - 1) * pageSize; // Tính toán số sách cần bỏ qua

			// Câu lệnh SQL với phân trang
			string sql = @"
        SELECT 
            S.MaSach, 
            S.TenSach, 
            S.GiaBan,
            ISNULL(S.Anh, '/images/defaultBook.webp') AS Anh,
            ISNULL(T.TenTg, 'Chưa có tác giả') AS TacGia,
            ISNULL(N.TenNxb, 'Chưa có nhà xuất bản') AS NhaXuatBan,
            ISNULL(L.TenTheLoai, 'Chưa có thể loại') AS TheLoai,
            k.SoLuongTon as SoLuong
        FROM Sach S
        LEFT JOIN TacGia T ON S.MaTacGia = T.MaTG
        LEFT JOIN NhaXuatBan N ON S.MaNxb = N.MaNxb
        LEFT JOIN TheLoai L ON S.MaTheLoai = L.MaTheLoai
        LEFT JOIN TonKho K ON S.MaSach= K.MaSach
        ORDER BY S.MaSach
        OFFSET @Skip ROWS
        FETCH NEXT @PageSize ROWS ONLY;
    ";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				await conn.OpenAsync();
				using (SqlCommand cmd = new SqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@Skip", skip);
					cmd.Parameters.AddWithValue("@PageSize", pageSize);

					using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							books.Add(new NewAppBookShop.ViewModels.BookViewModel
							{
								MaSach = reader.GetInt64(0),
								TenSach = reader.GetString(1),
								GiaBan = reader.GetDecimal(2),
								Anh = reader.GetString(3),
								TacGia = reader.GetString(4),
								NhaXuatBan = reader.GetString(5),
								TheLoai = reader.GetString(6),
								SoLuong = reader.GetInt32(7),
							});
						}
					}
				}
			}

			// Tính toán tổng số trang
			string countSql = @"
        SELECT COUNT(*) 
        FROM Sach S
        LEFT JOIN TacGia T ON S.MaTacGia = T.MaTG
        LEFT JOIN NhaXuatBan N ON S.MaNxb = N.MaNxb
        LEFT JOIN TheLoai L ON S.MaTheLoai = L.MaTheLoai
        LEFT JOIN TonKho K ON S.MaSach= K.MaSach;
    ";

			int totalBooks = 0;
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				await conn.OpenAsync();
				using (SqlCommand cmd = new SqlCommand(countSql, conn))
				{
					totalBooks = (int)await cmd.ExecuteScalarAsync();
				}
			}

			// Tính tổng số trang
			var totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

			// Lưu thông tin phân trang vào ViewBag
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewBag.Books = books;

			var topbooks = new List<NewAppBookShop.ViewModels.BookViewModel>();


			string sqlview = "select MaSach,TenSach,GiaBan,ISNULL(Anh, '/images/defaultBook.webp') AS Anh,TacGia,NhaXuatBan,TheLoai,TongSoLuongBan,SoLuongTon from Top5SachBanChay";  // Sử dụng View vBooks để truy vấn

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				await conn.OpenAsync();
				using (SqlCommand cmd = new SqlCommand(sqlview, conn))
				{
					using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							topbooks.Add(new NewAppBookShop.ViewModels.BookViewModel
							{
								MaSach = reader.GetInt64(0),
								TenSach = reader.GetString(1),
								GiaBan = reader.GetDecimal(2),
								Anh = reader.GetString(3),
								TacGia = reader.GetString(4),
								NhaXuatBan = reader.GetString(5),
								TheLoai = reader.GetString(6),
								SoLuongBan = reader.GetInt32(7),
								SoLuong = reader.GetInt32(8)

							});
						}
					}
				}
			}

			// Sử dụng ViewBag để truyền dữ liệu sang View
			ViewBag.topbooks = topbooks;

			return View();
		}


		[Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
