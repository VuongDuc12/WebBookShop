using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace NewAppBookShop.ViewModels
{
    public class BookViewModel
    {
        public long MaSach { get; set; }

        public string TenSach { get; set; } = null!;


        public string TacGia { get; set; }

        public string NhaXuatBan { get; set; }

        public string TheLoai { get; set; }

        public decimal GiaBan { get; set; }

        public bool TrangThai { get; set; }

        public string? Anh { get; set; }
		public int SoLuong { get; set; }
     
		public int SoLuongBan { get; set; }


	}

}

