using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class GioHang
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual AspNetUser User { get; set; } = null!;
}
