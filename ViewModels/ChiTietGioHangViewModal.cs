namespace NewAppBookShop.ViewModels
{
public class ChiTietGioHangViewModel
{
    public long MaSach { get; set; }
    public string TenSach { get; set; }
    public int SoLuong { get; set; }
    public decimal GiaBan { get; set; }
    public decimal ThanhTien { get; set; }  // Được tính trong SQL
}
}
