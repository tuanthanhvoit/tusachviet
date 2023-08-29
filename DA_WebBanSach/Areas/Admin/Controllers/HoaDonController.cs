using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;
using WebMatrix.WebData;
using DA_WebBanSach.Filters;
using RazorPDF;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin")]
    [InitializeSimpleMembershipAttribute]
    public class HoaDonController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/HoaDon/

        public ActionResult Index()
        {
            var hoadons = db.HoaDons.Include(h => h.NguoiDung).ToList();
            return View(hoadons);
        }

        //
        // GET: /Admin/HoaDon/Details/5

        public ActionResult Details(int id = 0)
        {
            HoaDon hoadon = db.HoaDons.Find(id);
            if (hoadon == null)
            {
                return HttpNotFound();
            }
            return View(hoadon);
        }

        //
        // GET: /Admin/HoaDon/Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemSach(string txtMaVach)
        {
            
            try
            {
                long id = long.Parse(txtMaVach);
                var sach = db.Saches.Where(s => s.Mavach == id).Single();
                if (sach.SoLuongTon <= 0)
                {
                    TempData["ErrorThemSach"] = "Số Lượng Sách Không Còn";
                    return RedirectToAction("Create");
                }
                GioHang.CartDHAdmin.ThemGio(sach.SachID);
            }
            catch (Exception)
            {
                TempData["ErrorThemSach"] = "Sách không tồn tại";
            }
            
            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult QuanLy(String btnAction = null)
        {
            if (btnAction == "Cập Nhật")
            {
                String[] names = Request.Form.AllKeys;
                foreach (var name in names)
                {
                    if (name.StartsWith("qty"))
                    {
                            int id = int.Parse(name.Substring(3));
                            if (Request.Form[name].Length < 3)
                            {
                                try
                                {
                                    int quantity = int.Parse(Request.Form[name]);
                                    GioHang.CartDHAdmin.Update(id, quantity);
                                }
                                catch (Exception)
                                {
                                    ViewBag.Error = "Số Lượng Không Đươc Nhập Chữ";
                                }
                                
                            }
                            else {
                                ViewBag.Error = "Số Lượng Nhập Quá Lớn";
                            }                     
                    }
                }
            }
            else if (btnAction == "Xóa Tất Cả")
            {
                GioHang.CartDHAdmin.Clear();
            }

            return RedirectToAction("Create");
        }

        public ActionResult Remove(int id = 0)
        {
            GioHang.CartDHAdmin.Remove(id);
            String response = String.Format("fnUpdateCartInfo({0}, {1})",
                GioHang.CartDHAdmin.Count, GioHang.CartDHAdmin.Amount);
            return Content(response);
        }

        public ActionResult InHoaDon(int id=0) {
            HoaDon hoadon = db.HoaDons.Find(id);
            if (hoadon == null)
            {
                return HttpNotFound();
            }
            return View(hoadon);
        }

        //
        // POST: /Admin/HoaDon/Create

        [HttpPost]
        public ActionResult Create(double txtTienKhachTra, double txtTienThoi)
        {
            if ((txtTienKhachTra - txtTienThoi) != GioHang.CartDHAdmin.Amount)
            {
                ViewBag.ErrorHoaDon = "Tiền Khách Trả và Tiền Thối Lại Phải Bằng Tổng Tiền.";
                return View();
            }

            HoaDon hd = new HoaDon { 
                NgayLapHD = DateTime.Now,
                TienKhachTra = (decimal)txtTienKhachTra,
                TienThoi = (decimal)txtTienThoi,
                TongTien = (decimal)GioHang.CartDHAdmin.Amount,
                UserId = WebSecurity.GetUserId(User.Identity.Name)
            };

            db.HoaDons.Add(hd);

            foreach (var item in GioHang.CartDHAdmin._Items)
	        {
                ChiTietHoaDon ct = new ChiTietHoaDon { 
                    HoaDonID = hd.HoaDonID,
                    SachID = item.SachID,
                    SoLuong = item.SoLuong,
                    DonGia = (decimal)item.Gia,
                    ThanhTien = (decimal)item.Gia * item.SoLuong
                };
                db.ChiTietHoaDons.Add(ct);
                TruSoLuong(ct.SachID, ct.SoLuong);
	        }

            try
            {
                db.SaveChanges();
                GioHang.CartDHAdmin.Clear();
            }
            catch (Exception)
            {
                ViewBag.ErrorHoaDon = "Lỗi Khi Thêm Hóa Đơn Vui Lòng Thử Lại";
                return View();
            }

            return RedirectToAction("Details", new { id=hd.HoaDonID});
        }

        //
        // GET: /Admin/HoaDon/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GioHangHD.CartAdmin.Clear();
            HoaDon hoadon = db.HoaDons.Find(id);
            if (hoadon == null)
            {
                return HttpNotFound();
            }
            try
            {
                var dhct = db.ChiTietHoaDons.Where(d => d.HoaDonID == id).Select(d => d).ToList();
                foreach (var ct in dhct)
                {
                    GioHangHD.CartAdmin.ThemGio(ct);
                }
            }
            catch
            {

            }
            return View(hoadon);
        }

        //
        // POST: /Admin/HoaDon/Edit/5

        [HttpPost]
        public ActionResult Edit(HoaDon hoadon)
        {
            if ((hoadon.TienKhachTra-hoadon.TienThoi) != hoadon.TongTien)
            {
                ViewBag.Error = "Tiền Khách Trả và Tiền Thối Lại Phải Bằng Tổng Tiền.";
                return View(hoadon);
            }

            if (ModelState.IsValid)
            {
                db.Entry(hoadon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.NguoiDungs, "UserId", "UserName", hoadon.UserId);
            return View(hoadon);
        }

        //
        // GET: /Admin/HoaDon/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HoaDon hoadon = db.HoaDons.Find(id);
            if (hoadon == null)
            {
                return HttpNotFound();
            }
            return View(hoadon);
        }

        //
        // POST: /Admin/HoaDon/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            HoaDon hoadon = db.HoaDons.Find(id);
            var ct = db.ChiTietHoaDons.Where(d => d.HoaDonID == id).ToList();
            foreach (var item in ct)
            {
                db.ChiTietHoaDons.Remove(item);
                themSoLuong(item.SachID, item.SoLuong);
            }
            db.HoaDons.Remove(hoadon);
            try
            {
                db.SaveChanges();
            }
            catch {
                TempData["ErrorXoaHD"] = "Lỗi Trong Quá Trình Xóa";
                return View("Delete", hoadon);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditChiTiet(int maHD, string btnAction = "Sửa")
        {
            HoaDon dh = db.HoaDons.Where(d => d.HoaDonID == maHD).Single();
            if (btnAction == "Sửa")
            {
                String[] names = Request.Form.AllKeys;
                foreach (var name in names)
                {
                    if (name.StartsWith("qty"))
                    {
                        //ma chi tiet
                        int id = int.Parse(name.Substring(3));
                        if (Request.Form[name].Length < 3)
                        {
                            try
                            {
                                int quantity = int.Parse(Request.Form[name]);
                                GioHangHD.CartAdmin.Update(id, quantity);
                                ChiTietHoaDon c = db.ChiTietHoaDons.Where(ct => ct.ChiTietHoaDonID == id).Single();
                                Sach s = db.Saches.Find(c.SachID);
                                if (s.SoLuongTon >= quantity)
                                {
                                    themSoLuong(c.SachID, c.SoLuong - quantity);
                                    c.SoLuong = quantity;
                                }
                            }
                            catch (Exception)
                            {
                                ViewBag.Error = "Số Lượng Không Đươc Nhập Chữ";
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Số Lượng Nhập Quá Lớn";
                        }
                    }
                }
                dh.TongTien = (decimal)GioHangHD.CartAdmin.Amount;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = maHD });
            }
            else
            {
                String[] names = Request.Form.AllKeys;
                foreach (var name in names)
                {
                    if (name.StartsWith("chk"))
                    {
                        int id = int.Parse(name.Substring(3));
                        GioHangHD.CartAdmin.Remove(id);
                        ChiTietHoaDon ct = db.ChiTietHoaDons.Find(id);
                        TruSoLuong(ct.SachID, ct.SoLuong);
                        db.ChiTietHoaDons.Remove(ct);
                    }
                }

                dh.TongTien = (decimal)GioHangHD.CartAdmin.Amount;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = maHD });
            }
        }

        public void themSoLuong(int? sachID, int sl)
        {
            Sach s = db.Saches.Find(sachID);
            s.SoLuongTon = s.SoLuongTon + sl;
            db.Entry(s).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void TruSoLuong(int? masach, int soluong)
        {
            Sach s = db.Saches.Find(masach);
            s.SoLuongTon = s.SoLuongTon - soluong;
            db.Entry(s).State = EntityState.Modified;
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}