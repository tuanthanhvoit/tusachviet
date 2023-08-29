using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Models
{

    public class TinTuc
    {
        [Key]
        public int TinTucID { get; set; }
        [Required(ErrorMessage="Chưa Nhập Tiêu Đề")]
        public string TieuDe { get; set; }
        [Required(ErrorMessage = "Chưa Nhập Hình")]
        public string UrlHinh { get; set; }
        [Required(ErrorMessage = "Chưa Nhập Tóm Tắt")]
        public string TomTat { get; set; }
        [Required(ErrorMessage = "Chưa Nhập Nội Dung")]
        public string NoiDung { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayViet { get; set; }
        public int UserId { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }

    public class TacGia
    {
        [Key]
        public int TacGiaID { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên tác giả")]
        [StringLength(50,ErrorMessage = ("Tên tác giả dài hơn 50 kí tự"))]
        public string TenTG { get; set; }
        [StringLength(255, ErrorMessage = ("Mô tả dài hơn 255 kí tự"))]
        public string GhiChu { get; set; }

        public virtual List<ChiTietTacGia> ChiTietTacGias { get; set; }
    }

    public class ChiTietTacGia
    {
        [Key]
        public int ChiTietTacGiaID { get; set; }
        public int TacGiaID { get; set; }
        public int SachID { get; set; }

        public virtual Sach Sach { get; set; }
        public virtual TacGia TacGia { get; set; }
    }

    public class ChungLoaiSach
    {
        [Key]
        public int ChungLoaiSachID { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên chủng loại sách")]
        [StringLength(50, ErrorMessage = ("Tên chủng loại sách dài hơn 50 kí tự"))]
        public string TenChungLoai { get; set; }

        public virtual List<LoaiSach> LoaiSaches { get; set; }
    }

    public class LoaiSach
    {
        [Key]
        public int LoaiSachID { get; set; }
        public int ChungLoaiSachID { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên loại sách")]
        [StringLength(50, ErrorMessage = ("Tên loại sách dài hơn 50 kí tự"))]
        public string TenLS { get; set; }
        [StringLength(255, ErrorMessage = ("Mô tả dài hơn 255 kí tự"))]
        public string GhiChu { get; set; }

        public virtual List<ChiTietLoaiSach> ChiTietLoaiSaches { get; set; }
        public virtual ChungLoaiSach ChungLoaiSach { get; set; }
    }

    public class ChiTietLoaiSach
    {
        [Key]
        public int ChiTietLoaiSachID { get; set; }
        public int LoaiSachID { get; set; }
        public int SachID { get; set; }

        public virtual LoaiSach LoaiSach { get; set; }
        public virtual Sach Sach { get; set; }
    }

    public class Sach
    {
        [Key]
        public int SachID { get; set; }
        public int NhaXuatBanID { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên sách")]
        [StringLength(255, ErrorMessage = ("Tên sách dài hơn 255 kí tự"))]
        public string TenSach { get; set; }
        [Required(ErrorMessage = "Chưa nhập đơn giá")]
        [Range(5000, 1000000, ErrorMessage="Đơn giá trên 5,000 và nhỏ hơn 1,000,000")]
        public double DonGia { get; set; }
        [Required(ErrorMessage = "Chưa có ảnh bìa")]
        public string AnhBia { get; set; }
        [Required(ErrorMessage = "Chưa nhập ngày xuất bản")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayXuatBan { get; set; }
        public bool LoaiBia { get; set; }
        [Required(ErrorMessage = "Chưa nhập tóm tắt")]
        [StringLength(255,ErrorMessage = "Tóm tắt quá 255 kí tự")]
        public string TomTat { get; set; }
        public int SoLuongTon { get; set; }
        public Nullable<long> Mavach { get; set; }

        public virtual NhaXuatBan NhaXuatBan { get; set; }
        public virtual List<ChiTietTacGia> ChiTietTacGias { get; set; }
        public virtual List<ChiTietLoaiSach> ChiTietLoaiSaches { get; set; }
        public virtual List<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public virtual List<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual List<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual List<KhoHang> KhoHangs { get; set; }
    }

    public class NhaXuatBan
    {
        [Key]
        public int NhaXuatBanID { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên nhà xuất bản")]
        [StringLength(255, ErrorMessage = ("Tên nhà xuất bản dài hơn 255 kí tự"))]
        public string TenNXB { get; set; }
        [Required(ErrorMessage = "Chưa nhập địa chỉ")]
        [StringLength(255, ErrorMessage = ("Địa chỉ nhà xuất bản dài hơn 255 kí tự"))]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Chưa nhập quận")]
        [StringLength(50, ErrorMessage = ("Quận dài hơn 255 kí tự"))]
        public string Quan { get; set; }
        [Required(ErrorMessage = "Chưa nhập thành phố")]
        [StringLength(50, ErrorMessage = ("Tên thành phố dài hơn 50 kí tự"))]
        public string ThanhPho { get; set; }
        [Required(ErrorMessage = "Chưa nhập điện thoại")]
        [RegularExpression(@"^^[0-9]{10,11}$", ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        [StringLength(11, ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        public string DienThoai { get; set; }
        [Required(ErrorMessage = "Chưa nhập email")]
        [EmailAddress(ErrorMessage="Không đúng định dạnh Email")]
        public string Email { get; set; }

        public virtual List<Sach> Saches { get; set; }
    }

    public class KhuyenMai
    {
        [Key]
        public int KhuyenMaiID { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên đợt khuyến mãi")]
        [StringLength(255, ErrorMessage = ("Tên đợt khuyến mãi dài hơn 255 kí tự"))]
        public string TenDotKM { get; set; }
        [Required(ErrorMessage = "Chưa nhập giá giảm")]
        [Range(0,1,ErrorMessage="Giá giảm từ 0,0 đến 1")]
        public double GiaGiam { get; set; }
        [Required(ErrorMessage = "Chưa nhập ngày bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayBatDau { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayKetThuc { get; set; }

        public virtual List<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
    }

    public class ChiTietKhuyenMai
    {
        [Key]
        public int ChiTietKhuyenMaiID { get; set; }
        public int KhuyenMaiID { get; set; }
        public int SachID { get; set; }

        public virtual Sach Sach { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
    }

    public class HoaDon
    {
        [Key]
        public int HoaDonID { get; set; }
        public decimal TongTien { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayLapHD { get; set; }
        [Required(ErrorMessage="Chưa nhập tiền khách trả")]
        [Range(1,double.MaxValue,ErrorMessage="Tiền Khách Trả Lớn Hơn 0")]
        public decimal TienKhachTra { get; set; }
        [Required(ErrorMessage = "Chưa nhập tiền thối - nhập 0 nếu khách hàng trả đúng tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Tiền Khách Trả Lớn Hơn 0")]
        public decimal TienThoi { get; set; }
        public int UserId { get; set; }

        public virtual List<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }

    public class ChiTietHoaDon
    {
        [Key]
        public int ChiTietHoaDonID { get; set; }
        [Range(0, 99, ErrorMessage = "Số lượng nhập từ 0 đến 99")]
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public int HoaDonID { get; set; }
        public int SachID { get; set; }

        public virtual HoaDon HoaDon { get; set; }
        public virtual Sach Sach { get; set; }
    }

    public class NhanVien
    {
        [Key]
        public int NhanVienID { get; set; }
        public int UserId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayVaoLam { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }

    public class KhachHang
    {
        [Key]
        public int KhachHangID { get; set; }
        public int UserId { get; set; }
        public int DonHangID { get; set; }
        public Nullable<decimal> TongTien { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }

    public class ChiTietDonHang
    {
        [Key]
        public int ChiTietDonHangID { get; set; }
        [Range(0, 99, ErrorMessage = "Số lượng nhập từ 0 đến 99")]
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public int DonHangID { get; set; }
        public int SachID { get; set; }

        public virtual DonHang DonHang { get; set; }
        public virtual Sach Sach { get; set; }
    }

    //tinh trang don hang 0 1 2 3 dat /thanh toan /giao /huy
    public class DonHang
    {
        [Key]
        public int DonHangID { get; set; }
        public decimal TongTien { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayDatHang { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayGiaoHang { get; set; }
        public int HinhThucThanhToan { get; set; }
        [Required(ErrorMessage="Chưa nhập địa chỉ giao")]
        [StringLength(255,ErrorMessage = "Địa chỉ giao 255 kí tự")]
        public string DiaDiemNhanHang { get; set; }
        [Required(ErrorMessage = "Chưa nhập quận")]
        [StringLength(50, ErrorMessage = "Quận 50 kí tự")]
        public string Quan { get; set; }
        [Required(ErrorMessage = "Chưa nhập thành phố")]
        [StringLength(50, ErrorMessage = "Thành phố 50 kí tự")]
        public string ThanhPho { get; set; }
        public int TinhTrangDonHang { get; set; }
        public string TenNhanVien { get; set; }
        public bool DoiThuong { get; set; }
        public int UserId { get; set; }

        public virtual List<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }

    public class KhoHang
    {
        [Key]
        public int KhoHangID { get; set; }
        [Required(ErrorMessage = "Chưa nhập ngày nhập")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayNhap { get; set; }
        [Required(ErrorMessage = "Chưa nhập số lượng")]
        [Range(0,int.MaxValue)]
        public int SoLuongNhap { get; set; }
        public int SachID { get; set; }

        public virtual Sach Sach { get; set; }
    }

    [Table("webpages_Membership")]
    public class webpages_Membership
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(128)]
        public string ConfirmationToken { get; set; }
        public bool? IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        [Required, StringLength(128)]
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        [Required, StringLength(128)]
        public string PasswordSalt { get; set; }
        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
    }

}