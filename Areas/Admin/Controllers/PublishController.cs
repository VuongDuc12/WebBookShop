using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewAppBookShop.Data;

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
        [Route("admin/publish")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
