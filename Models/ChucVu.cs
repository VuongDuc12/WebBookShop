using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class ChucVu
{
    public int MaChucVu { get; set; }

    public string? TenChucVu { get; set; }

    public string? MoTa { get; set; }

    public decimal? LuongChinhThuc { get; set; }

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
