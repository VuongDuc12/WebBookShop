using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewAppBookShop.Areas.Identity.Data;
using NewAppBookShop.Data;
using NewAppBookShop.Models;  // Thêm namespace chứa model KhachHang

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonHangController : Controller
    {
        private readonly ILogger<DonHangController> _logger;
        private readonly BookShopContext   _context;
         private readonly UserManager<APpUser> _userManager;

        // Constructor để inject logger và DbContext vào controller
        public DonHangController(ILogger<DonHangController> logger, BookShopContext context, UserManager<APpUser> userManager)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DanhSachDonHang()
        {
            var hoaDonMuas = await _context.HoaDonMuas
                .Include(h => h.MaKhNavigation)  // Liên kết với Khách Hàng
                .Include(h => h.MaNvNavigation)  // Liên kết với Nhân Viên (nếu có)
                .ToListAsync();

            return View(hoaDonMuas);
        }
        public async Task<IActionResult>  Create()
            {
                // Lấy danh sách sản phẩm từ database để hiển thị trong dropdown
                ViewBag.KhachHangs = _context.KhachHangs.ToList();
                ViewBag.Saches = _context.Saches.ToList();
             // Lấy tên nhân viên đăng nhập
                return View();
            }
        [HttpPost]
public async Task<IActionResult> Store(HoaDonMua hoaDon, List<long> productIds, List<int> quantities)
{
    long? userId = await GetCurrentUserIdAsync(); // Lấy mã nhân viên đang đăng nhập
    hoaDon.MaNv = userId;
    hoaDon.NgayMua = DateTime.Now;

    decimal tongTien = 0;
    int tongSoLuong = 0;

    for (int i = 0; i < productIds.Count; i++)
    {
        var product = await _context.Saches.FindAsync(productIds[i]); // Sử dụng FindAsync để gọi bất đồng bộ
        if (product != null)
        {
            tongTien += product.GiaBan * quantities[i];
            tongSoLuong += quantities[i];
        }
    }

    hoaDon.TongTien = tongTien;
    hoaDon.Soluong = tongSoLuong;
    hoaDon.TrangThai = "Chờ xử lý";

    _context.HoaDonMuas.Add(hoaDon);
    await _context.SaveChangesAsync(); // Gọi SaveChangesAsync thay vì SaveChanges

    for (int i = 0; i < productIds.Count; i++)
    {
        var chiTiet = new ChiTietHoaDonMua
        {
            SoHdmua = hoaDon.SoHdmua,
            MaSach = productIds[i],
            SoLuong = quantities[i],
            DonGia = (await _context.Saches.FindAsync(productIds[i])).GiaBan // Sử dụng FindAsync
        };
        _context.ChiTietHoaDonMuas.Add(chiTiet);
    }

    await _context.SaveChangesAsync(); // Gọi SaveChangesAsync sau khi thêm ChiTietHoaDonMua

    return RedirectToAction("Index");
}

private async Task<long?> GetCurrentUserIdAsync()
{
    // Lấy thông tin người dùng hiện tại
    var user = await _userManager.GetUserAsync(User);
    
    // Nếu không có người dùng, trả về null
    if (user == null)
    {
        return null;
    }

    // Lấy MaNv từ bảng NhanVien dựa trên UserId
    var nhanVien = await _context.NhanViens
                                  .FirstOrDefaultAsync(nv => nv.UserId == user.Id);

    // Trả về MaNv của nhân viên (nullable long)
    return nhanVien?.MaNv;
}

  public async Task<IActionResult> TraCuuDonHangAsync()
{
    
    var donHangList = await _context.HoaDonMuas
        .Include(d => d.MaKhNavigation)  // Nạp thông tin khách hàng (User) của MaKh
        .Include(d => d.MaNvNavigation)  // Nạp thông tin nhân viên (User) của MaNv
        .ToListAsync();

    if (donHangList == null || !donHangList.Any())
    {
        return View("NoData");
    }

    ViewData["ActiveMenu"] = "DonHang";
    return View(donHangList);  // Truyền đúng model vào view
}


        public IActionResult BaoCaoThongKe()
        {
          
            return View();
        }



public async Task<IActionResult> Details(long id)
{
    // Lấy thông tin chi tiết hóa đơn mua
    var chiTietHoaDon = _context.ChiTietHoaDonMuas
                                .Where(ct => ct.SoHdmua == id)
                                .Include(ct => ct.MaSachNavigation) // Thông tin sách
                    
                                .ToList();

    // Lấy hóa đơn mua (để cập nhật trạng thái)
   var hoaDonMua = await _context.HoaDonMuas
                .Where(h => h.SoHdmua == id)
                .Include(h => h.MaKhNavigation)  // Liên kết với Khách Hàng
                .Include(h => h.MaNvNavigation)  // Liên kết với Nhân Viên (nếu có)
                .FirstOrDefaultAsync();  
    // Tạo ViewModel để truyền cả chi tiết và hóa đơn
    var viewModel = new HoaDonDetailsViewModel
    {
        HoaDon = hoaDonMua,
        ChiTietHoaDon = chiTietHoaDon
    };

    return View(viewModel);
}

[HttpPost]
public IActionResult UpdateTrangThai(long soHdmua, string trangThai)
{
    _logger.LogInformation("Updating order with ID: {OrderId}, new status: {Status}", soHdmua, trangThai);

    // Retrieve the invoice from the database
    var hoaDon = _context.HoaDonMuas.FirstOrDefault(hd => hd.SoHdmua == soHdmua);

    // Check if the invoice exists
    if (hoaDon == null)
    {
        _logger.LogWarning("Invoice with ID {OrderId} not found.", soHdmua);
        return NotFound("Không tìm thấy hóa đơn.");
    }

    // Update the status
    hoaDon.TrangThai = trangThai;

    // Save changes to the database
    try
    {
        _context.SaveChanges();
        _logger.LogInformation("Order {OrderId} status updated to {Status}", soHdmua, hoaDon.TrangThai);

        TempData["SuccessMessage"] = "Cập nhật trạng thái thành công.";
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating order with ID {OrderId}.", soHdmua);
        TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật trạng thái.";
        return RedirectToAction("Index");
    }

    // Redirect to the detail view
    return RedirectToAction("Details", new { id = soHdmua });
}

}
}
