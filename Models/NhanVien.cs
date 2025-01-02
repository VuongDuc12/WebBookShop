﻿using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class NhanVien
{
    public long MaNv { get; set; }

    public string UserId { get; set; } = null!;

    public string TenNv { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public DateOnly NgayVaoLam { get; set; }

    public decimal Luong { get; set; }

    public virtual ICollection<HoaDonMua> HoaDonMuas { get; set; } = new List<HoaDonMua>();

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();

    public virtual AspNetUser User { get; set; } = null!;
}
