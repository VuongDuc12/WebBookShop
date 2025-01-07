using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class Sach
{
    public long MaSach { get; set; }

    public string TenSach { get; set; } = null!;

    public long MaTacGia { get; set; }

    public long MaNxb { get; set; }

    public int MaTheLoai { get; set; }

    public decimal GiaBan { get; set; }

    public bool TrangThai { get; set; }

    public string? Anh { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();

    public virtual NhaXuatBan MaNxbNavigation { get; set; } = null!;

    public virtual TacGium MaTacGiaNavigation { get; set; } = null!;

    public virtual TheLoai MaTheLoaiNavigation { get; set; } = null!;

    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
}
