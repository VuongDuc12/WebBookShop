using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class HoaDonMua
{
    public long SoHdmua { get; set; }

    public long MaKh { get; set; }

    public long? MaNv { get; set; }

    public DateTime NgayMua { get; set; }

    public decimal TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public int? Soluong { get; set; }

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual NhanVien? MaNvNavigation { get; set; }
}
