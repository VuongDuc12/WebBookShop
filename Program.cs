using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Data;
using NewAppBookShop.Areas.Identity.Data;
using NewAppBookShop.Areas.Identity.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối từ cấu hình
var connectionString = builder.Configuration.GetConnectionString("NewAppBookShopContextConnection")
    ?? throw new InvalidOperationException("Connection string 'NewAppBookShopContextConnection' not found.");

// Cấu hình DbContext
builder.Services.AddDbContext<NewAppBookShopContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Identity
builder.Services.AddIdentity<APpUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<NewAppBookShopContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();


    

// Thêm Razor Pages với hỗ trợ Microsoft Identity UI
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Khởi tạo vai trò bằng cách sử dụng IdentitySeeder


// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Đăng nhập trước
app.UseAuthorization();

// Cấu hình Endpoints
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Route mặc định cho các Controller bên ngoài Area
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Map Razor Pages
app.MapRazorPages();

app.Run();
