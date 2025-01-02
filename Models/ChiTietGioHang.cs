using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class ChiTietGioHang
{
    public long Id { get; set; }

    public long GioHangId { get; set; }

    public long MaSach { get; set; }

    public int SoLuong { get; set; }

    public decimal Gia { get; set; }

    public virtual GioHang GioHang { get; set; } = null!;

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
