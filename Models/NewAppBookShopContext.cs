using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NewAppBookShop.Models;

public partial class NewAppBookShopContext : DbContext
{
    public NewAppBookShopContext()
    {
    }

    public NewAppBookShopContext(DbContextOptions<NewAppBookShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietHoaDonMua> ChiTietHoaDonMuas { get; set; }

    public virtual DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HoaDonMua> HoaDonMuas { get; set; }

    public virtual DbSet<HoaDonNhap> HoaDonNhaps { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LichSuHoatDong> LichSuHoatDongs { get; set; }

    public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<TacGium> TacGia { get; set; }

    public virtual DbSet<TonKho> TonKhos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:NewAppBookShopContextConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietG__3214EC072B99F11B");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.GioHang).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.GioHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietGioHang_GioHang");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietGioHang_Sach");
        });

        modelBuilder.Entity<ChiTietHoaDonMua>(entity =>
        {
            entity.HasKey(e => new { e.SoHdmua, e.MaSach });

            entity.ToTable("ChiTietHoaDonMua");

            entity.Property(e => e.SoHdmua).HasColumnName("SoHDMua");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.ChiTietHoaDonMuas)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHoaDonMua_Sach");

            entity.HasOne(d => d.SoHdmuaNavigation).WithMany(p => p.ChiTietHoaDonMuas)
                .HasForeignKey(d => d.SoHdmua)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHoaDonMua_HoaDonMua");
        });

        modelBuilder.Entity<ChiTietHoaDonNhap>(entity =>
        {
            entity.HasKey(e => new { e.SoHdnhap, e.MaSach });

            entity.ToTable("ChiTietHoaDonNhap");

            entity.Property(e => e.SoHdnhap).HasColumnName("SoHDNhap");
            entity.Property(e => e.GiaNhap).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.ChiTietHoaDonNhaps)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHoaDonNhap_Sach");

            entity.HasOne(d => d.SoHdnhapNavigation).WithMany(p => p.ChiTietHoaDonNhaps)
                .HasForeignKey(d => d.SoHdnhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHoaDonNhap_HoaDonNhap");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GioHang__3214EC07F17C1F36");

            entity.ToTable("GioHang");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GioHang_AspNetUsers");
        });

        modelBuilder.Entity<HoaDonMua>(entity =>
        {
            entity.HasKey(e => e.SoHdmua).HasName("PK__HoaDonMu__057986317CC446DC");

            entity.ToTable("HoaDonMua");

            entity.Property(e => e.SoHdmua).HasColumnName("SoHDMua");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayMua)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDonMuas)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDonMua_KhachHang");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonMuas)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK_HoaDonMua_NhanVien");
        });

        modelBuilder.Entity<HoaDonNhap>(entity =>
        {
            entity.HasKey(e => e.SoHdnhap).HasName("PK__HoaDonNh__8D63C188A775671B");

            entity.ToTable("HoaDonNhap");

            entity.Property(e => e.SoHdnhap).HasColumnName("SoHDNhap");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDonNhap_NhanVien");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1EBDC80659");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenKh)
                .HasMaxLength(255)
                .HasColumnName("TenKH");
            entity.Property(e => e.TongChiTieu).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KhachHang_AspNetUsers");
        });

        modelBuilder.Entity<LichSuHoatDong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuHo__3214EC0739F76463");

            entity.ToTable("LichSuHoatDong");

            entity.Property(e => e.DiaChiIp)
                .HasMaxLength(45)
                .HasColumnName("DiaChiIP");
            entity.Property(e => e.LoaiHoatDong).HasMaxLength(255);
            entity.Property(e => e.ThoiGianHoatDong)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.LichSuHoatDongs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichSuHoatDong_AspNetUsers");
        });

        modelBuilder.Entity<NhaXuatBan>(entity =>
        {
            entity.HasKey(e => e.MaNxb).HasName("PK__NhaXuatB__3A19482CA0555C33");

            entity.ToTable("NhaXuatBan");

            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNxb)
                .HasMaxLength(255)
                .HasColumnName("TenNXB");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70AA0B059E1");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.Luong).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayVaoLam).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNv)
                .HasMaxLength(255)
                .HasColumnName("TenNV");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NhanVien_AspNetUsers");
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742D46F869A4");

            entity.ToTable("Sach");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.TenSach).HasMaxLength(255);
            entity.Property(e => e.TheLoai).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaNxbNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNxb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sach_NhaXuatBan");

            entity.HasOne(d => d.MaTacGiaNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaTacGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sach_TacGia");
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.MaTg).HasName("PK__TacGia__272500749B099699");

            entity.Property(e => e.MaTg).HasColumnName("MaTG");
            entity.Property(e => e.TenTg)
                .HasMaxLength(255)
                .HasColumnName("TenTG");
        });

        modelBuilder.Entity<TonKho>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TonKho__3214EC074E82A709");

            entity.ToTable("TonKho");

            entity.Property(e => e.LanCapNhatCuoi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.TonKhos)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TonKho_Sach");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
