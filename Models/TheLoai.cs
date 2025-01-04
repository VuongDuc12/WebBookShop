using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class TheLoai
{
    public int MaTheLoai { get; set; }

    public string? TenTheLoai { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
