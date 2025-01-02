using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace NewAppBookShop.Areas.Admin.ViewModel
{
    public class RoleViewModel: PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleViewModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<IdentityRole> roles { get; set; } = new List<IdentityRole>(); // Khởi tạo danh sách rỗng

        [TempData]
        public string StatusMessage { get; set; } = string.Empty; // Gán giá trị mặc định

        public async Task<IActionResult> OnGet()
        {
            try
            {
                roles = await _roleManager.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi khi tải danh sách roles: {ex.Message}";
            }

            return Page();
        }
    }

}

