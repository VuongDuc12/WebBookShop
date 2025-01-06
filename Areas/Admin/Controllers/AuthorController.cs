using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewAppBookShop.Data;

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
        public IActionResult Index()
        {
            return View();
        }
    }
}
