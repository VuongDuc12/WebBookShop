using NewAppBookShop.Models;

public class HoaDonDetailsViewModel
{
    public HoaDonMua HoaDon { get; set; } = null!;
    public List<ChiTietHoaDonMua> ChiTietHoaDon { get; set; } = new List<ChiTietHoaDonMua>();
}
