using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;
using DA_WebBanSach.Filters;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin")]
    public class DonHangController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/DonHang/

        private List<SelectListItem> hinhthucthanhtoan = new List<SelectListItem>()
        {
            new SelectListItem() {Text="Thu Tiền Tại Nhà", Value="0"},
            new SelectListItem() {Text="Ngân Lượng", Value="1"}
        };

        private List<SelectListItem> tinhtrangdonhang = new List<SelectListItem>()
        {
            new SelectListItem() {Text="Hủy", Value="-1"},
            new SelectListItem() {Text="Chưa Thanh Toán", Value="0"},
            new SelectListItem() {Text="Chờ Giao Hàng", Value="1"},
            new SelectListItem() {Text="Đã Giao Hàng", Value="2"},
        };

        public ActionResult Index()
        {
            ViewBag.HinhThucThanhToan = new SelectList(hinhthucthanhtoan, "Value", "Text");
            ViewBag.TinhTrangDonHang = new SelectList(tinhtrangdonhang, "Value", "Text");
            var donhangs = db.DonHangs.Include(d => d.NguoiDung);
            return View(donhangs.ToList());
        }

        //
        // GET: /Admin/DonHang/Details/5

        public ActionResult Details(int id = 0)
        {
            DonHang donhang = db.DonHangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            try
            {
                ViewBag.TongSoLuong = db.ChiTietDonHangs.Where(d => d.DonHangID == id).Sum(d => d.SoLuong).ToString();
            }
            catch (Exception)
            {
                ViewBag.TongSoLuong = 0;
            }
            
            ViewBag.HinhThucThanhToan = new SelectList(hinhthucthanhtoan, "Value", "Text");
            ViewBag.TinhTrangDonHang = new SelectList(tinhtrangdonhang, "Value", "Text");
            return View(donhang);
        }

        //
        // GET: /Admin/DonHang/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GioHangAdmin.CartAdmin.Clear();
            DonHang donhang = db.DonHangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            try
            {
                var dhct = db.ChiTietDonHangs.Where(d => d.DonHangID == id).Select(d => d).ToList();
                foreach (var ct in dhct)
                {
                    GioHangAdmin.CartAdmin.ThemGio(ct);
                }
            }
            catch { 
                
            }
            ViewBag.HinhThucThanhToan = new SelectList(hinhthucthanhtoan, "Value", "Text");
            ViewBag.TinhTrangDonHang = new SelectList(tinhtrangdonhang, "Value", "Text",donhang.TinhTrangDonHang);
            return View(donhang);
        }

        //
        // POST: /Admin/DonHang/Edit/5

        [HttpPost]
        public ActionResult Edit(DonHang donhang)
        {
            ViewBag.HinhThucThanhToan = new SelectList(hinhthucthanhtoan, "Value", "Text", donhang.HinhThucThanhToan);
            ViewBag.TinhTrangDonHang = new SelectList(tinhtrangdonhang, "Value", "Text", donhang.TinhTrangDonHang);
            donhang.TongTien = (decimal)GioHangAdmin.CartAdmin.Amount;
            if (ModelState.IsValid)
            {
                if (donhang.TenNhanVien==null)
                {
                    donhang.TenNhanVien = User.Identity.Name;
                }

                if (donhang.TinhTrangDonHang == 2)
                {
                    try
                    {
                        KhachHang tichdiem = db.KhachHangs.Where(t => t.UserId == donhang.UserId && t.DonHangID == donhang.DonHangID).SingleOrDefault();
                        if (donhang.DoiThuong == true)
                        {
                            tichdiem.TongTien = -1000000;
                        }
                        else
                        {
                            tichdiem.TongTien = donhang.TongTien;
                        }
                    }
                    catch
                    {
                        KhachHang tichdiem = new KhachHang { 
                            UserId = donhang.UserId,
                            DonHangID = donhang.DonHangID,
                            TongTien = donhang.TongTien
                        };
                        db.KhachHangs.Add(tichdiem);
                    }
                }
                else
                {
                    try
                    {
                        KhachHang tichdiem = db.KhachHangs.Where(t => t.UserId == donhang.UserId && t.DonHangID == donhang.DonHangID).SingleOrDefault();
                        db.KhachHangs.Remove(tichdiem);
                    }
                    catch 
                    { 
                    
                    }
                }

                db.Entry(donhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id=donhang.DonHangID});
            }
            return View(donhang);
        }

        public void themSoLuong(int? sachID, int sl)
        {
            Sach s = db.Saches.Find(sachID);
            s.SoLuongTon = s.SoLuongTon + sl;
            db.Entry(s).State = EntityState.Modified;
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult EditChiTiet(int maDH, string btnAction = "Sửa")
        {
            DonHang dh = db.DonHangs.Where(d => d.DonHangID == maDH).Single();
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
                                GioHangAdmin.CartAdmin.Update(id, quantity);
                                ChiTietDonHang c = db.ChiTietDonHangs.Where(ct => ct.ChiTietDonHangID == id).Single();
                                Sach s = db.Saches.Find(c.SachID);
                                if (s.SoLuongTon >= quantity)
                                {
                                    themSoLuong(c.SachID, c.SoLuong - quantity);
                                    c.SoLuong = quantity;
                                }
                            }
                            catch (Exception)
                            {
                                TempData["Error"] = "Số Lượng Không Đươc Nhập Chữ";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Số Lượng Nhập Quá Lớn";
                        }
                    }
                }
                dh.TongTien = (decimal)GioHangAdmin.CartAdmin.Amount;
                dh.TenNhanVien = User.Identity.Name;            
            }
            else 
            {
                    String[] names = Request.Form.AllKeys;
                    foreach (var name in names)
                    {
                        if (name.StartsWith("chk"))
                        {
                            int id = int.Parse(name.Substring(3));
                            GioHangAdmin.CartAdmin.Remove(id);
                            ChiTietDonHang ct = db.ChiTietDonHangs.Find(id);
                            themSoLuong(ct.SachID,ct.SoLuong);
                            db.ChiTietDonHangs.Remove(ct);
                        }
                    }

                    if (GioHangAdmin.CartAdmin.Count == 0)
                    {
                        dh.TinhTrangDonHang = -1;
                        try
                        {
                            KhachHang tichdiem = db.KhachHangs.Where(t => t.UserId == dh.UserId && t.DonHangID == dh.DonHangID).SingleOrDefault();
                            db.KhachHangs.Remove(tichdiem);
                        }
                        catch
                        {

                        }
                    }
                    dh.TongTien = (decimal)GioHangAdmin.CartAdmin.Amount;
                    dh.TenNhanVien = User.Identity.Name;
            }

            if (dh.TinhTrangDonHang == 2)
            {
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
                    if (dh.DoiThuong == true)
                    {
                        KhachHang tichdiem = new KhachHang
                        {
                            UserId = dh.UserId,
                            DonHangID = dh.DonHangID,
                            TongTien = -1000000
                        };
                        db.KhachHangs.Add(tichdiem);
                    }
                    else
                    {
                        KhachHang tichdiem = new KhachHang
                        {
                            UserId = dh.UserId,
                            DonHangID = dh.DonHangID,
                            TongTien = dh.TongTien
                        };
                        db.KhachHangs.Add(tichdiem);
                    }                   
                }
            }

            db.SaveChanges();
            return RedirectToAction("Edit", new { id = maDH });
        }

        [HttpPost, ActionName("Index")]
        public ActionResult remove() {
            String[] names = Request.Form.AllKeys;
            var listdh = db.DonHangs.Where(d => d.TinhTrangDonHang == -1).ToList();
            foreach (var item in names)
            {
                if (item.StartsWith("chk"))
                {
                    int id = int.Parse(item.Substring(3));
                    try
                    {
                            var dh = listdh.Where(s => s.DonHangID == id).Single();
                            if (dh.ChiTietDonHangs.Count == 0)
                            {
                                db.DonHangs.Remove(dh);
                            }
                            else {
                                foreach (var ct in db.ChiTietDonHangs.Where(c => c.DonHangID == id).ToList())
                                {
                                    db.ChiTietDonHangs.Remove(ct);
                                    themSoLuong(ct.SachID, ct.SoLuong);
                                }
                                db.DonHangs.Remove(dh);
                            }
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Đơn Hàng Không Xóa Được Vì Tình Trạng Thanh Toán Không Phải Hủy";
                        return View(db.DonHangs.ToList());
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}