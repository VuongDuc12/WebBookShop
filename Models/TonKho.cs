using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class TonKho
{
    public long Id { get; set; }

    public long MaSach { get; set; }

    public int SoLuongTon { get; set; }

    public DateTime LanCapNhatCuoi { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
