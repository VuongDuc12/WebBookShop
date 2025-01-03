using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Models;  // Thêm namespace chứa model KhachHang

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly BookShopContext _context;

        public CustomerController(BookShopContext context)
        {
            _context = context;
        }

        // Action để hiển thị danh sách khách hàng
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả khách hàng từ bảng KhachHang
            var customers = await _context.KhachHangs.Include(k => k.User).ToListAsync();
            return View(customers);  // Truyền danh sách khách hàng vào view
        }

        // Action để chỉnh sửa thông tin khách hàng
        public async Task<IActionResult> Edit(long id)
        {
            var customer = await _context.KhachHangs.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);  // Truyền khách hàng cần chỉnh sửa vào view
        }

        // Action để lưu thay đổi sau khi chỉnh sửa thông tin khách hàng
        [HttpPost]
        public async Task<IActionResult> Edit(KhachHang customer)
        {
            if (ModelState.IsValid)
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Quay lại danh sách khách hàng
            }
            return View(customer);  // Trả về view nếu có lỗi
        }


        public IActionResult Create()
{
    return View();  // Hiển thị form thêm khách hàng
}

[HttpPost]
public async Task<IActionResult> Create(KhachHang customer)
{
    if (ModelState.IsValid)
    {
        _context.KhachHangs.Add(customer);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));  // Quay lại trang danh sách khách hàng
    }
    return View(customer);  // Nếu có lỗi, quay lại form nhập
}
    }
}
