-- Bảng BacKhachHang
CREATE TABLE BacKhachHang (
    MaBac INT PRIMARY KEY,
    TenBac NVARCHAR(255),
    MucGiamGia FLOAT
);

-- Bảng ChucVu
CREATE TABLE ChucVu (
    MaChucVu INT PRIMARY KEY,
    TenChucVu NVARCHAR(255),
    MoTa NVARCHAR(MAX),
    LuongChinhThuc DECIMAL(18, 2)
);

-- Bảng NhanVien
CREATE TABLE NhanVien (
    MaNv BIGINT PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    TenNv NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(MAX),
    SoDienThoai NVARCHAR(15),
    NgaySinh DATE,
    NgayVaoLam DATE NOT NULL,
    LuongCoBan DECIMAL(18, 2) NOT NULL,
    MaChucVu INT,
    TrangThai BIT,
	FOREIGN KEY (MaChucVu) REFERENCES ChucVu(MaChucVu),
	 FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Bảng KhachHang
CREATE TABLE KhachHang (
    MaKh BIGINT PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    TenKh NVARCHAR(255) NOT NULL,
    SoDienThoai NVARCHAR(15) NOT NULL,
    GioiTinh NVARCHAR(50),
    NgaySinh DATE,
    DiaChi NVARCHAR(MAX),
    TongChiTieu DECIMAL(18, 2) NOT NULL,
    MaBac INT,
    FOREIGN KEY (MaBac) REFERENCES BacKhachHang(MaBac),
	FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Bảng HoaDonNhap
CREATE TABLE HoaDonNhap (
    SoHdnhap BIGINT PRIMARY KEY,
    MaNv BIGINT NOT NULL,
    NgayNhap DATETIME NOT NULL,
    TongTien DECIMAL(18, 2) NOT NULL,
    TrangThai NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_HoaDonNhap_NhanVien FOREIGN KEY (MaNv) REFERENCES NhanVien(MaNv)
);

-- Bảng HoaDonMua
CREATE TABLE HoaDonMua (
    SoHdmua BIGINT PRIMARY KEY,
    MaKh BIGINT NOT NULL,
    MaNv BIGINT,
    NgayMua DATETIME NOT NULL,
    TongTien DECIMAL(18, 2) NOT NULL,
    TrangThai NVARCHAR(255) NOT NULL,
    Soluong INT,
    CONSTRAINT FK_HoaDonMua_KhachHang FOREIGN KEY (MaKh) REFERENCES KhachHang(MaKh),
    CONSTRAINT FK_HoaDonMua_NhanVien FOREIGN KEY (MaNv) REFERENCES NhanVien(MaNv)
);

-- Bảng NhaXuatBan
CREATE TABLE NhaXuatBan (
    MaNxb BIGINT PRIMARY KEY,
    TenNxb NVARCHAR(255) NOT NULL,
    SoDienThoai NVARCHAR(15),
    DiaChi NVARCHAR(MAX)
);

-- Bảng TacGia
CREATE TABLE TacGia (
    MaTg BIGINT PRIMARY KEY,
    TenTg NVARCHAR(255) NOT NULL
);

-- Bảng TheLoai
CREATE TABLE TheLoai (
    MaTheLoai INT PRIMARY KEY,
    TenTheLoai NVARCHAR(255)
);

-- Bảng Sach
CREATE TABLE Sach (
    MaSach BIGINT PRIMARY KEY,
    TenSach NVARCHAR(255) NOT NULL,
    MaTacGia BIGINT NOT NULL,
    MaNxb BIGINT NOT NULL,
    MaTheLoai INT NOT NULL,
    GiaBan DECIMAL(18, 2) NOT NULL,
    TrangThai BIT NOT NULL,
    Anh NVARCHAR(MAX),
    CONSTRAINT FK_Sach_TacGia FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTg),
    CONSTRAINT FK_Sach_NhaXuatBan FOREIGN KEY (MaNxb) REFERENCES NhaXuatBan(MaNxb),
    CONSTRAINT FK_Sach_TheLoai FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai)
);

-- Bảng TonKho
CREATE TABLE TonKho (
    Id BIGINT PRIMARY KEY,
    MaSach BIGINT NOT NULL,
    SoLuongTon INT NOT NULL,
    LanCapNhatCuoi DATETIME NOT NULL,
    CONSTRAINT FK_TonKho_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng GioHang
CREATE TABLE GioHang (
    Id BIGINT PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    NgayTao DATETIME NOT NULL,
	 TrangThai NVARCHAR(255) NOT NULL,
	 TongSoLuong INT DEFAULT 0, -- Tổng số lượng sản phẩm, mặc định là 0
	 GiamGia Float NULL,
    TongThanhTien DECIMAL(18, 2) DEFAULT 0.0
);

-- Bảng ChiTietGioHang
CREATE TABLE ChiTietGioHang (
    Id BIGINT PRIMARY KEY,
    GioHangId BIGINT NOT NULL,
    MaSach BIGINT NOT NULL,
    SoLuong INT NOT NULL,
    Gia DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_ChiTietGioHang_GioHang FOREIGN KEY (GioHangId) REFERENCES GioHang(Id),
    CONSTRAINT FK_ChiTietGioHang_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng ChiTietHoaDonMua
CREATE TABLE ChiTietHoaDonMua (
    SoHdmua BIGINT NOT NULL,
    MaSach BIGINT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (SoHdmua, MaSach),
    CONSTRAINT FK_ChiTietHoaDonMua_HoaDonMua FOREIGN KEY (SoHdmua) REFERENCES HoaDonMua(SoHdmua),
    CONSTRAINT FK_ChiTietHoaDonMua_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng ChiTietHoaDonNhap
CREATE TABLE ChiTietHoaDonNhap (
    SoHdnhap BIGINT NOT NULL,
    MaSach BIGINT NOT NULL,
    SoLuong INT NOT NULL,
    GiaNhap DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (SoHdnhap, MaSach),
    CONSTRAINT FK_ChiTietHoaDonNhap_HoaDonNhap FOREIGN KEY (SoHdnhap) REFERENCES HoaDonNhap(SoHdnhap),
    CONSTRAINT FK_ChiTietHoaDonNhap_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng LichSuHoatDong
CREATE TABLE LichSuHoatDong (
    Id BIGINT PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    LoaiHoatDong NVARCHAR(255) NOT NULL,
    ThoiGianHoatDong DATETIME NOT NULL,
    DiaChiIp NVARCHAR(255)
);
-- Thêm dữ liệu cho bảng BacKhachHang
INSERT INTO BacKhachHang (MaBac, TenBac, MucGiamGia) VALUES 
(1, N'Bạc', 0.05), 
(2, N'Vàng', 0.10), 
(3, N'Kim cương', 0.15);

-- Thêm dữ liệu cho bảng ChucVu
INSERT INTO ChucVu (MaChucVu, TenChucVu, MoTa, LuongChinhThuc) VALUES 
(1, N'Nhân viên bán hàng', N'Mô tả công việc bán hàng', 8000000), 
(2, N'Quản lý', N'Mô tả công việc quản lý', 12000000);

-- Thêm dữ liệu cho bảng NhanVien
INSERT INTO NhanVien (MaNv, UserId, TenNv, DiaChi, SoDienThoai, NgaySinh, NgayVaoLam, LuongCoBan, MaChucVu, TrangThai) VALUES 
(1, N'NV01', N'Nguyễn Văn A', N'Hà Nội', '0987654321', '1990-01-01', GETDATE(), 9000000, 1, 1);

-- Thêm dữ liệu cho bảng KhachHang
INSERT INTO KhachHang (MaKh, UserId, TenKh, SoDienThoai, GioiTinh, NgaySinh, DiaChi, TongChiTieu, MaBac) VALUES 
(1, N'KH01', N'Trần Văn B', '0981111111', N'Nam', '1985-05-15', N'Hồ Chí Minh', 0, 1);

-- Thêm dữ liệu cho bảng NhaXuatBan
INSERT INTO NhaXuatBan (MaNxb, TenNxb, SoDienThoai, DiaChi) VALUES 
(1, N'NXB Kim Đồng', '0912345678', N'Hà Nội');

-- Thêm dữ liệu cho bảng TacGia
INSERT INTO TacGia (MaTg, TenTg) VALUES 
(1, N'Nguyễn Nhật Ánh');

-- Thêm dữ liệu cho bảng TheLoai
INSERT INTO TheLoai (MaTheLoai, TenTheLoai) VALUES 
(1, N'Tiểu thuyết'), 
(2, N'Truyện ngắn');

-- Thêm dữ liệu cho bảng Sach
INSERT INTO Sach (MaSach, TenSach, MaTacGia, MaNxb, MaTheLoai, GiaBan, TrangThai, Anh) VALUES 
(1, N'Tôi thấy hoa vàng trên cỏ xanh', 1, 1, 1, 80000, 1, NULL);

-- Thêm dữ liệu cho bảng TonKho
INSERT INTO TonKho (Id, MaSach, SoLuongTon, LanCapNhatCuoi) VALUES 
(1, 1, 50, GETDATE());

-- Thêm dữ liệu cho bảng GioHang
INSERT INTO GioHang (Id, UserId, NgayTao, TrangThai, TongSoLuong, GiamGia, TongThanhTien) VALUES 
(1, N'KH01', GETDATE(), N'DangDatHang', 0, NULL, 0);

-- Thêm dữ liệu cho bảng ChiTietGioHang
INSERT INTO ChiTietGioHang (Id, GioHangId, MaSach, SoLuong, Gia) VALUES 
(1, 1, 1, 2, 80000);

-- Thêm dữ liệu cho bảng HoaDonMua
INSERT INTO HoaDonMua (SoHdmua, MaKh, MaNv, NgayMua, TongTien, TrangThai, Soluong) VALUES 
(1, 1, 1, GETDATE(), 160000, N'DaThanhToan', 2);

-- Thêm dữ liệu cho bảng ChiTietHoaDonMua
INSERT INTO ChiTietHoaDonMua (SoHdmua, MaSach, SoLuong, DonGia) VALUES 
(1, 1, 2, 80000);

-- Thêm dữ liệu cho bảng HoaDonNhap
INSERT INTO HoaDonNhap (SoHdnhap, MaNv, NgayNhap, TongTien, TrangThai) VALUES 
(1, 1, GETDATE(), 400000, N'Hoàn thành');

-- Thêm dữ liệu cho bảng ChiTietHoaDonNhap
INSERT INTO ChiTietHoaDonNhap (SoHdnhap, MaSach, SoLuong, GiaNhap) VALUES 
(1, 1, 10, 40000);


Drop proc sp_LayIdGioHang

CREATE PROCEDURE sp_LayIdGioHang --Nếu chưa có giỏ hàng đang tồn tại thì tạo mới giỏ hàng r kaays ra idgiohang
    @UserId NVARCHAR(255),
    @GioHangId BIGINT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Kiểm tra xem giỏ hàng chưa thanh toán đã tồn tại chưa
        IF EXISTS (
            SELECT 1 FROM GioHang 
            WHERE UserId = @UserId AND TrangThai = N'Đang đặt hàng'
        )
        BEGIN
            -- Lấy giỏ hàng hiện có
            SELECT @GioHangId = Id 
            FROM GioHang 
            WHERE UserId = @UserId AND TrangThai = N'Đang đặt hàng';
        END
        ELSE
        BEGIN
            -- Tạo giỏ hàng mới
            INSERT INTO GioHang (UserId, NgayTao, TrangThai)
            VALUES (@UserId, GETDATE(), N'Đang đặt hàng');

            -- Lấy ID của giỏ hàng vừa tạo
            SET @GioHangId = SCOPE_IDENTITY();
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
drop proc sp_ThemSanPhamVaoGioHang
CREATE PROCEDURE sp_ThemSanPhamVaoGioHang --thêm mới sản phẩm vào giỏ hàng được lưu vào bảng chitietgiohang
    @GioHangId BIGINT,
    @MaSach BIGINT,
    @SoLuong INT
    
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
		Declare @Gia decimal(18, 2)

		SELECT @Gia = GiaBan FROM Sach 
            WHERE MaSach = @MaSach
        -- Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
        IF EXISTS (
            SELECT 1 FROM ChiTietGioHang 
            WHERE GioHangId = @GioHangId AND MaSach = @MaSach
        )
        BEGIN
            -- Cập nhật số lượng và giá tiền trong chi tiết giỏ hàng
            UPDATE ChiTietGioHang
            SET SoLuong = SoLuong + @SoLuong,
                Gia = @Gia
            WHERE GioHangId = @GioHangId AND MaSach = @MaSach;
        END
        ELSE
        BEGIN
            -- Thêm sản phẩm mới vào chi tiết giỏ hàng
            INSERT INTO ChiTietGioHang (GioHangId, MaSach, SoLuong, Gia)
            VALUES (@GioHangId, @MaSach, @SoLuong, @Gia);
        END

        -- Cập nhật tổng số lượng và thành tiền của giỏ hàng
        UPDATE GioHang
        SET TongSoLuong = TongSoLuong + @SoLuong,
            TongThanhTien = TongThanhTien + (@SoLuong * @Gia)
        WHERE Id = @GioHangId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

drop proc sp_ThanhToanGioHang 

CREATE PROCEDURE sp_ThanhToanGioHang
    @GioHangId BIGINT,
    @MaKh BIGINT, -- Mã khách hàng
    @MaNv BIGINT, -- Mã nhân viên (nếu có)
    @SoHdmua BIGINT OUTPUT -- Số hóa đơn mua mới sẽ trả về
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Lấy thông tin tổng tiền và giảm giá từ giỏ hàng
        DECLARE @TongTien DECIMAL(18, 2), @TongSoLuong INT;
        SELECT @TongTien = t.TongThanhTien , @TongSoLuong = TongSoLuong
        FROM GioHang t
        WHERE Id = @GioHangId;
		

        -- 2. Tạo hóa đơn mua
        INSERT INTO HoaDonMua (MaKh, NgayMua, TongTien, TrangThai, Soluong)
        VALUES (@MaKh, GETDATE(), @TongTien, N'Chờ xử lý', @TongSoLuong);

        -- Lấy số hóa đơn mua mới
        SET @SoHdmua = SCOPE_IDENTITY();
        DECLARE @MaSach BIGINT, @SoLuong INT, @Gia DECIMAL(18, 2);
        DECLARE cur CURSOR FOR 
            SELECT MaSach, SoLuong, Gia
            FROM ChiTietGioHang
            WHERE GioHangId = @GioHangId;

        OPEN cur;
        FETCH NEXT FROM cur INTO @MaSach, @SoLuong, @Gia;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            INSERT INTO ChiTietHoaDonMua (SoHdmua, MaSach, SoLuong, DonGia)
            VALUES (@SoHdmua, @MaSach, @SoLuong, @Gia);

            FETCH NEXT FROM cur INTO @MaSach, @SoLuong, @Gia;
        END

        CLOSE cur;
        DEALLOCATE cur;

        UPDATE GioHang
        SET TrangThai = N'Đã Đặt Hàng'
        WHERE Id = @GioHangId;
        DELETE FROM ChiTietGioHang
        WHERE GioHangId = @GioHangId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

CREATE FUNCTION fn_LaySoLuongDonMuaThanhCong()
RETURNS INT
AS
BEGIN
    DECLARE @SoLuongDon INT;

    -- Đếm số lượng hóa đơn mua
    SELECT @SoLuongDon = COUNT(*)
    FROM HoaDonMua where TrangThai = N'Thành Công';

    -- Trả về giá trị 0 nếu không có hóa đơn
    RETURN ISNULL(@SoLuongDon, 0);
END;


CREATE FUNCTION fn_LayTongDoanhThu()
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @TongDoanhThu DECIMAL(18, 2);

    -- Tính tổng doanh thu từ cột 'TongTien' trong bảng HoaDonMua
    SELECT @TongDoanhThu = SUM(TongTien)
    FROM HoaDonMua where TrangThai = 'Thành Công';

    -- Trả về giá trị 0 nếu không có doanh thu
    RETURN ISNULL(@TongDoanhThu, 0);
END;



CREATE VIEW v_DanhSachHoaDonKhachHang AS
SELECT 
    kh.MaKh AS MaKhachHang,
    kh.TenKh AS TenKhachHang, -- Chỉ bao gồm nếu bảng KhachHang có cột này
    hdm.SoHdmua AS SoHoaDon,
    hdm.NgayMua,
    hdm.TongTien,
    hdm.TrangThai
FROM 
    KhachHang kh
JOIN 
    HoaDonMua hdm
ON 
    kh.MaKh = hdm.MaKh;



CREATE VIEW v_TongTienTheoKhachHang
AS
SELECT 
    MaKh,
    COUNT(SoHdmua) AS SoHoaDon,
    SUM(TongTien) AS TongTien
FROM HoaDonMua
GROUP BY MaKh;


CREATE VIEW v_HoaDonChuaThanhToan
AS
SELECT 
    SoHdmua,
    NgayMua,
    MaKh,
    TongTien
FROM HoaDonMua
WHERE TrangThai = 'ChuaThanhToan';

CREATE TRIGGER trg_CapNhatTongChiTieuSauThanhToan
ON HoaDonMua
AFTER UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM INSERTED
        WHERE TrangThai = N'Thành Công'
    )
    BEGIN
        UPDATE KhachHang
        SET TongChiTieu = TongChiTieu + i.TongTien
        FROM KhachHang kh
        INNER JOIN INSERTED i ON kh.MaKh = i.MaKh
        WHERE i.TrangThai = N'Thành Công';
    END
END;
CREATE TRIGGER trg_CapNhatKhoSauThemHoaDonMua
ON ChiTietHoaDonMua
AFTER INSERT
AS
BEGIN
  
    UPDATE T
    SET 
        T.SoLuongTon = T.SoLuongTon - I.SoLuong,
        T.LanCapNhatCuoi = GETDATE() 
    FROM TonKho T
    JOIN INSERTED I ON T.MaSach = I.MaSach;

END;


CREATE VIEW v_Top5DonHangGiaTriCaoNhat AS
SELECT TOP 5
    hdm.SoHdmua AS SoHoaDon,
    kh.TenKh AS TenKhachHang,
    hdm.NgayMua,
    hdm.TongTien
FROM 
    HoaDonMua hdm
JOIN 
    KhachHang kh ON hdm.MaKh = kh.MaKh
WHERE 
    MONTH(hdm.NgayMua) = MONTH(GETDATE()) 
    AND YEAR(hdm.NgayMua) = YEAR(GETDATE())
ORDER BY 
    hdm.TongTien DESC;


CREATE VIEW v_KhachHangCoNhieuDonHangnhat AS
SELECT 
    TOP 5 
    K.MaKh, 
    K.TenKh,
    COUNT(HDM.SoHdmua) AS SoLuongDonHang
FROM 
    KhachHang K
JOIN 
    HoaDonMua HDM ON K.MaKh = HDM.MaKh
WHERE 
    MONTH(HDM.NgayMua) = MONTH(GETDATE()) 
    AND YEAR(HDM.NgayMua) = YEAR(GETDATE())
	AND HDM.TrangThai = N'Thành Công'
GROUP BY 
    K.MaKh, K.TenKh
ORDER BY 
    SoLuongDonHang DESC;



SELECT dbo.fn_LaySoLuongDonMuaThanhCong()
SELECT dbo.fn_LayTongDoanhThu()