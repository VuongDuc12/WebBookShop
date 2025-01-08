using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Data;
using NewAppBookShop.Models;

namespace NewAppBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly BookShopContext _context;


        public EmployeeController(BookShopContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            // Lấy tất cả nhân viên từ bảng NhanVien
            var employee = await _context.NhanViens.Include(k => k.User).Include(nv => nv.MaChucVuNavigation).ToListAsync();
            return View(employee);  // Truyền danh sách nhân viên vào view
        }

        [Route("admin/employee/Analytics")]
        public async Task<IActionResult> Analytics()
        {
            return View(); 
        }
        [Route("admin/employee/Payslip")]
        public async Task<IActionResult> Payslip()
        {
            return View();
        }

    }
}
