using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class LichSuHoatDong
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public string LoaiHoatDong { get; set; } = null!;

    public DateTime ThoiGianHoatDong { get; set; }

    public string? DiaChiIp { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
