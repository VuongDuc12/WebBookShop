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
using NewAppBookShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối từ cấu hình
var connectionString = builder.Configuration.GetConnectionString("NewAppBookShopContextConnection")
    ?? throw new InvalidOperationException("Connection string 'NewAppBookShopContextConnection' not found.");

// Cấu hình DbContext cho NewAppBookShopContext và BookShopContext
builder.Services.AddDbContext<NewAppBookShopContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<BookShopContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Identity với APpUser và IdentityRole
builder.Services.AddIdentity<APpUser, IdentityRole>(options =>
{
    // Cấu hình yêu cầu xác nhận tài khoản khi đăng nhập
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<NewAppBookShopContext>()  // Kết nối Identity với DbContext
.AddDefaultUI()  // Sử dụng UI mặc định của Identity
.AddDefaultTokenProviders();  // Cung cấp các phương thức tạo mã thông báo mặc định

// Thêm Razor Pages với hỗ trợ Microsoft Identity UI
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Thêm Controllers với Razor Runtime Compilation cho phép chỉnh sửa Razor Views khi chạy ứng dụng
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Khởi tạo vai trò nếu cần
// Bạn có thể thêm logic để khởi tạo vai trò ở đây, ví dụ như sử dụng một service hoặc một method khởi tạo.


// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Cấu hình lỗi
    app.UseHsts();  // Kích hoạt HTTP Strict Transport Security (HSTS) cho môi trường production
}

app.UseHttpsRedirection();  // Chuyển hướng tất cả các yêu cầu HTTP sang HTTPS
app.UseStaticFiles();  // Sử dụng các file tĩnh (CSS, JS, ảnh, v.v.)

app.UseRouting();  // Sử dụng hệ thống routing của ASP.NET Core
app.UseAuthentication();  // Thực hiện xác thực người dùng
app.UseAuthorization();  // Thực hiện phân quyền người dùng

// Cấu hình các route của ứng dụng
// Định tuyến cho các Controllers trong Area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Định tuyến mặc định cho Controllers bên ngoài Area
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Định tuyến Razor Pages
app.MapRazorPages();

app.Run();
