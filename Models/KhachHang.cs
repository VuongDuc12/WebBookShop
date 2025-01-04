using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class KhachHang
{
    public long MaKh { get; set; }

    public string UserId { get; set; } = null!;

    public string TenKh { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public decimal TongChiTieu { get; set; }

    public int? MaBac { get; set; }

    public virtual ICollection<HoaDonMua> HoaDonMuas { get; set; } = new List<HoaDonMua>();

    public virtual BacKhachHang? MaBacNavigation { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
