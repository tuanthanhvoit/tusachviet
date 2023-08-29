using DA_WebBanSach.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace DA_WebBanSach.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("SachDBTest")
        {
        }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
    }
    
    [Table("NguoiDungs")]
    public class NguoiDung
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required(ErrorMessage="Chưa Nhập Username")]
        [StringLength(25,ErrorMessage="Tên đăng nhập quá 25 kí tự")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Chưa Nhập Email")]
        [EmailAddress(ErrorMessage="Email sai định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Chưa Nhập họ tên")]
        public string HoTen { get; set; }
        public bool GioiTinh { get; set; }
        [Required(ErrorMessage = "Chưa nhập ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgaySinh { get; set; }
        [Required(ErrorMessage = "Chưa nhập điện thoại")]
        [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        [StringLength(11, ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        public string DienThoai { get; set; }
        [Required(ErrorMessage = "Chưa nhập địa chỉ")]
        [StringLength(255, ErrorMessage = ("Địa chỉ nhà xuất bản dài hơn 255 kí tự"))]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Chưa nhập quận")]
        [StringLength(50, ErrorMessage = ("Quận dài hơn 255 kí tự"))]
        public string Quan { get; set; }
        [Required(ErrorMessage = "Chưa nhập thành phố")]
        [StringLength(50, ErrorMessage = ("Tên thành phố dài hơn 50 kí tự"))]
        public string ThanhPho { get; set; }

        public virtual List<DonHang> DonHangs { get; set; }
        public virtual List<HoaDon> HoaDons { get; set; }
        public virtual List<KhachHang> KhachHangs { get; set; }
        public virtual List<NhanVien> NhanViens { get; set; }
        public virtual List<TinTuc> TinTucs { get; set; }
        
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class ConfirmModel
    {
        [Required(ErrorMessage="Chưa nhập email.")]
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        public string ChuoiConfirm { get; set; }

        [Required(ErrorMessage="Chưa Nhập Mật Khẩu")]
        [StringLength(25, ErrorMessage = "Mật khẩu phải từ {0} đến {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không giống nhau vui lòng kiếm tra lại")]
        public string ConfirmPassword { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "Chưa Nhập Mật Khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Mật Khẩu Mới")]
        [StringLength(25, ErrorMessage = "Mật khẩu phải từ {0} đến {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không giống nhau vui lòng kiếm tra lại.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Chưa Nhập Username")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Mật Khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {

        [Required(ErrorMessage="Chưa Nhập Username")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Chưa nhập mật khẩu")]
        [StringLength(25, ErrorMessage = "Mật khẩu phải từ 6 đến 25 kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau vui lòng kiếm tra lại.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Email")]
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }
    }

    public class RegisterModelFull
    {

        [Required(ErrorMessage="Chưa Nhập Username")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Họ Tên")]
        [Display(Name = "Họ Tên")]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; }

        [Required(ErrorMessage = "Chưa nhập ngày sinh - dd/mm/yyyy")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Sinh")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Chưa nhập điện thoại")]
        [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        [StringLength(11, ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        [Display(Name = "Điện Thoại")]
        public string DienThoai { get; set; }

        [Required(ErrorMessage = "Chưa nhập địa chỉ")]
        [StringLength(255, ErrorMessage = ("Địa chỉ nhà xuất bản dài hơn 255 kí tự"))]
        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chưa nhập quận")]
        [StringLength(50, ErrorMessage = ("Quận dài hơn 255 kí tự"))]
        public string Quan { get; set; }

        [Required(ErrorMessage = "Chưa nhập thành phố")]
        [StringLength(50, ErrorMessage = ("Tên thành phố dài hơn 50 kí tự"))]
        [Display(Name = "Thành Phố")]
        public string ThanhPho { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Mật Khẩu")]
        [StringLength(25, ErrorMessage = "Mật khẩu phải từ {0} đến {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau vui lòng kiếm tra lại.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Chưa nhập email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage="Không đúng định dạng Email")]
        public string Email { get; set; }
    }

    public class NhanVienModel
    {

        [Required(ErrorMessage = "Chưa Nhập Username")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Họ Tên")]
        [Display(Name = "Họ Tên")]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; }

        [Required(ErrorMessage = "Chưa nhập ngày sinh - dd/mm/yyyy")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Sinh")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Chưa nhập điện thoại")]
        [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        [StringLength(11, ErrorMessage = "Số điện thoại từ 10 đến 11 chữ số - Nhập mã vùng nếu là điện thoại bàn")]
        [Display(Name = "Điện Thoại")]
        public string DienThoai { get; set; }

        [Required(ErrorMessage = "Chưa nhập địa chỉ")]
        [StringLength(255, ErrorMessage = ("Địa chỉ nhà xuất bản dài hơn 255 kí tự"))]
        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chưa nhập quận")]
        [StringLength(50, ErrorMessage = ("Quận dài hơn 255 kí tự"))]
        [Display(Name = "Quận")]
        public string Quan { get; set; }

        [Required(ErrorMessage = "Chưa nhập thành phố")]
        [StringLength(50, ErrorMessage = ("Tên thành phố dài hơn 50 kí tự"))]
        [Display(Name = "Thành Phố")]
        public string ThanhPho { get; set; }
        [Required(ErrorMessage = "Chưa nhập email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
