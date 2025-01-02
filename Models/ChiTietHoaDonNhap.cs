using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class ChiTietHoaDonNhap
{
    public long SoHdnhap { get; set; }

    public long MaSach { get; set; }

    public int SoLuong { get; set; }

    public decimal GiaNhap { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;

    public virtual HoaDonNhap SoHdnhapNavigation { get; set; } = null!;
}
