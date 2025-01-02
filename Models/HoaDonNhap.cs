using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class HoaDonNhap
{
    public long SoHdnhap { get; set; }

    public long MaNv { get; set; }

    public DateTime NgayNhap { get; set; }

    public decimal TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
