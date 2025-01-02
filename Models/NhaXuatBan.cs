using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class NhaXuatBan
{
    public long MaNxb { get; set; }

    public string TenNxb { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
