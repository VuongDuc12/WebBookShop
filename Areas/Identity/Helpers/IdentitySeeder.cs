using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace NewAppBookShop.Areas.Identity.Helpers;

// Add profile data for application users by adding properties to the APpUser class
 public static class IdentitySeeder
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "NhanVien", "KhachHang" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
      

}