using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using DA_WebBanSach.Filters;
using System.Data;

namespace DA_WebBanSach.Controllers
{
    [Authorize]
    [InitializeSimpleMembershipAttribute]
    public class ThanhToanController : Controller
    {
        //
        // GET: /ThanhToan/

        private List<SelectListItem> hinhthuc = new List<SelectListItem>() { 
            new SelectListItem(){Text="Thu Tiền Tại Nhà", Value="0"},
            new SelectListItem(){Text="Ngân Lượng", Value="1"},
        };
        
        SachDbContext db = new SachDbContext();

        public ActionResult Index()
        {
            if (GioHang.Cart.Count==0)
            {
                return RedirectToAction("Index", "Home");
            }
            var s = db.NguoiDungs.Find(WebSecurity.GetUserId(User.Identity.Name));
            if (s.HoTen == null)
            {
                TempData["ErrorNoName"] = "** Vui Lòng Cập Nhật Thông Tin Cá Nhân";
                return RedirectToAction("SuaThongTinCaNhan", "Account", new { id = WebSecurity.GetUserId(User.Identity.Name) });
            }
            ViewBag.HinhThucThanhToan = new SelectList(hinhthuc, "Value", "Text");
            return View();
        }

        public ActionResult ThanhToan(DonHang dh)
        {
            ViewBag.HinhThucThanhToan = new SelectList(hinhthuc, "Value", "Text", dh.HinhThucThanhToan);
            String body = "Bạn vừa đặt mua 1 đơn hàng tại tusachviet.org \n";

            try
            {
                dh.UserId = WebSecurity.GetUserId(User.Identity.Name);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/ThanhToan/ThanhToan" });
            }
            
            dh.NgayDatHang = DateTime.Now;
            if (dh.HinhThucThanhToan == 0)
            {
                dh.NgayGiaoHang = DateTime.Now.AddDays(3);
            }
            else
            {
                dh.NgayGiaoHang = DateTime.Now.AddDays(7);
            }

            dh.TinhTrangDonHang = 0;

            if (dh.DoiThuong == true)
            {
                try
                {
                    decimal? dt = db.KhachHangs.Where(l => l.UserId == dh.UserId).Sum(l => l.TongTien);
                    if (dt < 1000000)
	                {
		                TempData["ErrorDT"] = "Điểm Thưởng Không Đủ Để Đổi Thưởng";
                        return RedirectToAction("Index");
	                }else
	                {
                        dh.TongTien = (decimal)(GioHang.Cart.Amount * 0.9);
	                }
                }
                catch{
                     TempData["ErrorDT"] = "Điểm Thưởng Không Đủ Để Đổi Thưởng";
                     return RedirectToAction("Index");
                }
            }
            else
            {
                dh.TongTien = (decimal)GioHang.Cart.Amount;
            }

            db.DonHangs.Add(dh);
            body = body + "TỔng Tiền: " + dh.TongTien + "\n" + "Tổng Số Lượng Sách: " + GioHang.Cart.Count+"\n";
            foreach (var item in GioHang.Cart._Items)
            {
                var ct = new ChiTietDonHang { 
                    DonGia = (decimal)item.Gia,
                    SoLuong = item.SoLuong,
                    ThanhTien = (decimal)(item.SoLuong*item.Gia),
                    DonHangID = dh.DonHangID,
                    SachID = item.SachID
                };
                body = body + "Sách: " + item.TenSach + "\t Số Lượng: " + item.SoLuong + "\t Đơn Giá: " + item.Gia + "\t Thành Tiền: " + item.SoLuong * item.Gia + "\n";
                db.ChiTietDonHangs.Add(ct);
                TruSoLuong(ct.SachID, ct.SoLuong);
            }
            try
            {
                db.SaveChanges();
                if (dh.DoiThuong==true)
                {
                    KhachHang diemthuong = new KhachHang
                    {
                        UserId = dh.UserId,
                        DonHangID = dh.DonHangID,
                        TongTien = -1000000
                    };
                    db.KhachHangs.Add(diemthuong);
                    db.SaveChanges();
                }
                NguoiDung nd = db.NguoiDungs.Find(dh.UserId);
                
                SentMail.GuiDonHang("admin@tusachviet.org", nd.Email, "Bạn Đã Đặt Một Đơn Hàng Tại tusachviet.org", body);
                GioHang.Cart.Clear();
                if (dh.HinhThucThanhToan==0)
                {
                    return RedirectToAction("XemDonHang", "Account");
                }
                else
                {
                    String return_url = "http://" + Request.Url.Authority + "/ThanhToan/ThanhToanThanhCong/";// Địa chỉ trả về 
                    String transaction_info = "Thanh toán mua hàng tại tusachviet.org";//Thông tin giao dịch
                    String order_code = dh.DonHangID.ToString();//Mã hoa don 
                    String receiver = "tuanthanhvo.1989@gmail.com";//Tài khoản nhận tiền 
                    String price = dh.TongTien.ToString();

                    NganLuong nl = new NganLuong();
                    string url = nl.buildCheckoutUrl(return_url, receiver, transaction_info, order_code, price);
                    return Redirect(url);
                }
            }
            catch {
                TempData["ErrorTT"] = "Vui Lòng Kiểm Tra Địa Chỉ Giao Hàng";
                return RedirectToAction("Index", "ThanhToan");
            }
        }

        public void TruSoLuong(int? masach, int soluong) {
            Sach s = db.Saches.Find(masach);
            s.SoLuongTon = s.SoLuongTon - soluong;
            db.Entry(s).State = EntityState.Modified;
            db.SaveChanges();
        }

        [AllowAnonymous]
        public ActionResult ThanhToanThanhCong(string id)
        {
            if (id=="")
            {
                return RedirectToAction("Index", "Home");
            }

            String transaction_info = Request.QueryString["transaction_info"];
            String order_code = Request.QueryString["order_code"];
            String payment_id = Request.QueryString["payment_id"];
            String payment_type = Request.QueryString["payment_type"];
            String secure_code = Request.QueryString["secure_code"];
            String price = Request.QueryString["price"];
            String error_text = Request.QueryString["error_text"];
            NganLuong nl = new NganLuong();
            bool check = nl.verifyPaymentUrl(transaction_info, order_code, price, payment_id, payment_type, error_text, secure_code);
            if (check)
            {
                int madh = int.Parse(order_code);
                decimal tt = decimal.Parse(price);
                DonHang dh = db.DonHangs.Find(madh);
                if (dh != null && dh.TongTien == tt)
                {
                    dh.TinhTrangDonHang = 2;
                    try
                    {
                        KhachHang tichdiem = db.KhachHangs.Where(t => t.UserId == dh.UserId && t.DonHangID == dh.DonHangID).SingleOrDefault();
                        if (dh.DoiThuong == true)
                        {
                            tichdiem.TongTien = -1000000;
                        }
                        else
                        {
                            tichdiem.TongTien = dh.TongTien;
                        }
                    }
                    catch
                    {
                        KhachHang tichdiem = new KhachHang
                        {
                            UserId = dh.UserId,
                            DonHangID = dh.DonHangID,
                            TongTien = dh.TongTien
                        };
                        db.KhachHangs.Add(tichdiem);
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch {
                        ViewBag.Error = "Thanh Toán Thành Công TuSachViet sẽ giao hàng cho bạn sớm nhất trong 7 ngày.";
                        return View();
                    }
                    
                    ViewBag.Error = "Thanh Toán Thành Công TuSachViet sẽ giao hàng cho bạn sớm nhất trong 7 ngày.";
                }
                else {
                    ViewBag.Error = "Lỗi Trong Quá Trình Thanh Toán Xem Lại Hoặc Liên Hệ Hỗ Trợ Để Được Giải Quyết Sớm";
                }
                
            }
            else {
                ViewBag.Error = "Lỗi Trong Quá Trình Thanh Toán Xem Lại Hoặc Liên Hệ Hỗ Trợ Để Được Giải Quyết Sớm";
            }
            return View();
        }
    }
}
