using DA_WebBanSach.Filters;
using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Areas.QuanLy.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin,NhanVien,BanHang,HoaDon")]
    public class ThongKeController : Controller
    {
        //
        // GET: /QuanLy/ThongKe/
        private SachDbContext db = new SachDbContext();

        public ActionResult Index()
        {

            DateTime now = DateTime.Now, nowy, nowyy;

            nowy = now.AddMonths(-1);
            nowyy = now.AddMonths(-2);

            DateTime ngaydau = GetFirstDayOfMonth(now);
            DateTime ngaycuoi = GetLastDayOfMonth(now);

            DateTime ngaydauy = GetFirstDayOfMonth(nowy);
            DateTime ngaycuoiy = GetLastDayOfMonth(nowy);

            DateTime ngaydauyy = GetFirstDayOfMonth(nowyy);
            DateTime ngaycuoiyy = GetLastDayOfMonth(nowyy);
            
            
            var data1 = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang >= ngaydau  && d.DonHang.NgayDatHang <= ngaycuoi)
                    .GroupBy(d => d.DonHang.DonHangID)
                    .Select(g => new Report
                    {
                        IName = g.Key,
                        TongSL = g.Sum(d => d.SoLuong),
                        TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                    }).ToList();

            var data2 = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang >= ngaydauy && d.DonHang.NgayDatHang <= ngaycuoiy)
                    .GroupBy(d => d.DonHang.DonHangID)
                    .Select(g => new Report
                    {
                        IName = g.Key,
                        TongSL = g.Sum(d => d.SoLuong),
                        TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                    }).ToList();

            var data3 = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang >= ngaydauyy && d.DonHang.NgayDatHang <= ngaycuoiyy)
                     .GroupBy(d => d.DonHang.DonHangID)
                     .Select(g => new Report
                     {
                         IName = g.Key,
                         TongSL = g.Sum(d => d.SoLuong),
                         TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                     }).ToList();

            var hd1 = db.ChiTietHoaDons.Where(d => d.HoaDon.NgayLapHD >= ngaydau && d.HoaDon.NgayLapHD <= ngaycuoi)
                    .GroupBy(d => d.HoaDon.HoaDonID)
                    .Select(g => new Report
                    {
                        IName = g.Key,
                        TongSL = g.Sum(d => d.SoLuong),
                        TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                    }).ToList();

            var hd2 = db.ChiTietHoaDons.Where(d => d.HoaDon.NgayLapHD >= ngaydauy && d.HoaDon.NgayLapHD <= ngaycuoiy)
                    .GroupBy(d => d.HoaDon.HoaDonID)
                    .Select(g => new Report
                    {
                        IName = g.Key,
                        TongSL = g.Sum(d => d.SoLuong),
                        TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                    }).ToList();

            var hd3 = db.ChiTietHoaDons.Where(d => d.HoaDon.NgayLapHD >= ngaydauyy && d.HoaDon.NgayLapHD <= ngaycuoiyy)
                     .GroupBy(d => d.HoaDon.HoaDonID)
                     .Select(g => new Report
                     {
                         IName = g.Key,
                         TongSL = g.Sum(d => d.SoLuong),
                         TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                     }).ToList();

            List<TongKetThang> tk = new List<TongKetThang>() { 
                new TongKetThang{Thang = nowyy.Month,TongSL = data3.Sum(d => d.TongSL)+hd3.Sum(e=>e.TongSL),TongTien = data3.Sum(d => d.TongTien)+hd3.Sum(e=>e.TongTien)},
                new TongKetThang{Thang = nowy.Month,TongSL = data2.Sum(d => d.TongSL)+hd2.Sum(e=>e.TongSL),TongTien = data2.Sum(d => d.TongTien)+hd2.Sum(e=>e.TongTien)},
                new TongKetThang{Thang = now.Month,TongSL = data1.Sum(d => d.TongSL)+hd1.Sum(e=>e.TongSL),TongTien = data1.Sum(d => d.TongTien)+hd1.Sum(e=>e.TongTien)}
            };

            ViewBag.DH = tk;

            var Sachbanchay = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang >= ngaydau && d.DonHang.NgayDatHang <= ngaycuoi).GroupBy(c => c.Sach).Select(g => new { Sach = g.Key, Tong = g.Sum(c => c.SoLuong) }).OrderByDescending(i => i.Tong).Select(c => new SoLuongSachBan { TenSach=c.Sach.TenSach, Count=c.Tong}).Take(10).ToList();    

            ViewBag.Sach = Sachbanchay;

            return View();
        }

        [HttpPost]
        public ActionResult ThongKeQuy(string QuyThang = "", string QuyNam = "2013")
        {
            try
            {
                int qn = int.Parse(QuyNam);
                if (qn < 2000)
                {
                    TempData["Error"] = "Không Có Dữ Liệu Năm" + qn;
                    return View("Index");
                }
                
                if (QuyThang == "")
                {
                    var hd2 = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang.Year == qn)
                     .GroupBy(d => (int)Math.Ceiling(1.0 * d.DonHang.NgayDatHang.Month / 3))
                     .Select(g => new Report2
                     {
                         name = g.Key,
                         TongSL = g.Sum(d => d.SoLuong),
                         TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                     }).ToList();

                    ViewBag.Tong = hd2.Sum(d => d.TongTien).ToString();
                    ViewBag.HDnam = hd2;
                    ViewBag.Nam = QuyNam;
                    return View();
                }
                else {
                    int qt = int.Parse(QuyThang);

                    if (qt < 0 && qt >= 5)
                    {
                        TempData["Error"] = "Nhập Quý Sai Vui Lòng Nhập Lại";
                        return View("Index");
                    }

                    DateTime ngaybd = DateTime.Now;
                    DateTime ngaykt = DateTime.Now;
                    switch (qt)
                    {
                        case 1:
                            ngaybd = Convert.ToDateTime(QuyNam + "/01/01");
                            ngaykt = Convert.ToDateTime(QuyNam + "/03/31");
                            break;
                        case 2:
                            ngaybd = Convert.ToDateTime(QuyNam + "/04/01");
                            ngaykt = Convert.ToDateTime(QuyNam + "/06/30");
                            break;
                        case 3:
                            ngaybd = Convert.ToDateTime(QuyNam + "/07/01");
                            ngaykt = Convert.ToDateTime(QuyNam + "/09/30");
                            break;
                        case 4:
                            ngaybd = Convert.ToDateTime(QuyNam + "/10/01");
                            ngaykt = Convert.ToDateTime(QuyNam + "/12/31");
                            break;
                    }

                    var hd3 = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang.Year == qn && d.DonHang.NgayDatHang >= ngaybd && d.DonHang.NgayDatHang <= ngaykt)
                         .GroupBy(d => d.DonHang.NgayDatHang.Month)
                         .Select(g => new Report2
                         {
                             name = g.Key,
                             TongSL = g.Sum(d => d.SoLuong),
                             TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                         }).ToList();
                    ViewBag.Tong = hd3.Sum(d => d.TongTien).ToString();
                    ViewBag.HD = hd3;
                    ViewBag.Quy = QuyThang;
                    ViewBag.Nam = QuyNam;
                    return View();
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Nhập Quý Sai Vui Lòng Nhập Lại";
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Index(NhapThongKe model, string btnAction = "Thống Kê Quý")
        {
                if (model.NgayBatDau==null)
                {
                    model.NgayBatDau = DateTime.Now;
                }
                if (model.NgayKetThuc==null)
                {
                    model.NgayKetThuc = model.NgayBatDau;
                }
                if (DateTime.Compare(model.NgayBatDau,model.NgayKetThuc)>0)
                {
                    ViewBag.Error = "Ngày Bắt Đầu Lớn Hơn Ngày Kết Thúc";
                    return View();
                }

                var hd3 = db.ChiTietDonHangs.Where(d => d.DonHang.NgayDatHang >= model.NgayBatDau && d.DonHang.NgayDatHang <= model.NgayKetThuc)
                     .GroupBy(d => d.DonHang.NgayDatHang.Day)
                     .Select(g => new Report2
                     {
                         name = g.Key,
                         TongSL = g.Sum(d => d.SoLuong),
                         TongTien = (double)g.Sum(d => d.DonGia * d.SoLuong)
                     }).ToList();
                ViewBag.HDThang = hd3;
            return View();
        }

        public DateTime GetFirstDayOfMonth(DateTime dtInput)
        {
            DateTime dtResult = dtInput;
            dtResult = dtResult.AddDays((-dtResult.Day) + 1);
            return dtResult;
        }

        public static DateTime GetLastDayOfMonth(DateTime dtInput)
        {
            DateTime dtResult = dtInput;
            dtResult = dtResult.AddMonths(1);
            dtResult = dtResult.AddDays(-(dtResult.Day));
            return dtResult;
        }

    }
}
