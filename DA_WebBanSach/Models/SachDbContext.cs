using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class SachDbContext:DbContext
    {
        public SachDbContext() : base("name=SachDBTest") {
        }

        public DbSet<TinTuc> TinTucs { get; set; }
        public DbSet<Sach> Saches { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<ChiTietTacGia> ChiTietTacGias { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<ChungLoaiSach> ChungLoaiSaches { get; set; }
        public DbSet<LoaiSach> LoaiSaches { get; set; }
        public DbSet<ChiTietLoaiSach> ChiTietLoaiSaches { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public DbSet<KhoHang> KhoHangs { get; set; }

        
    }
}