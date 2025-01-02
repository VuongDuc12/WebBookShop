using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class ChiTietHoaDonMua
{
    public long SoHdmua { get; set; }

    public long MaSach { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;

    public virtual HoaDonMua SoHdmuaNavigation { get; set; } = null!;
}
