using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class BacKhachHang
{
    public int MaBac { get; set; }

    public string? TenBac { get; set; }

    public double? MucGiamGia { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
