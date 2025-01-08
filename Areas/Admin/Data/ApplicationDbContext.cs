using Microsoft.EntityFrameworkCore;
using NewAppBookShop.Areas.Admin.Models;


namespace NewAppBookShop.Areas.Admin.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor nhận DbContextOptions
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> NhanViens { get; set; }
    }
}
