namespace NewAppBookShop.Areas.Admin.Models
{
    public class Employee
    {
        public int MaNV { get; set; }
        public string UserId { get; set; }
        public string TenNV { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgaySinh { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public decimal LuongCoBan { get; set; }
        public int MaChucVu { get; set; }
        public bool TrangThai { get; set; }
    }
}
        