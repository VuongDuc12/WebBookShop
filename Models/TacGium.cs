using System;
using System.Collections.Generic;

namespace NewAppBookShop.Models;

public partial class TacGium
{
    public long MaTg { get; set; }

    public string TenTg { get; set; } = null!;

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
