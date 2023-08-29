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
    public class KhoHangController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/KhoHang/

        public ActionResult Index()
        {
            var khohangs = db.KhoHangs.Include(k => k.Sach);
            return View(khohangs.ToList());
        }

        //
        // GET: /Admin/KhoHang/Details/5

        public ActionResult Details(int id = 0)
        {
            KhoHang khohang = db.KhoHangs.Find(id);
            if (khohang == null)
            {
                return HttpNotFound();
            }
            return View(khohang);
        }

        //
        // GET: /Admin/KhoHang/Create

        public ActionResult Create()
        {
            ViewBag.SachID = new SelectList(db.Saches, "SachID", "TenSach");
            return View();
        }

        //
        // POST: /Admin/KhoHang/Create

        [HttpPost]
        public ActionResult Create(KhoHang khohang)
        {
            if (ModelState.IsValid)
            {
                db.KhoHangs.Add(khohang);
                Sach s = db.Saches.Find(khohang.SachID);
                s.SoLuongTon += khohang.SoLuongNhap;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SachID = new SelectList(db.Saches, "SachID", "TenSach", khohang.SachID);
            return View(khohang);
        }

        //
        // GET: /Admin/KhoHang/Edit/5

        public ActionResult Edit(int id = 0)
        {
            KhoHang khohang = db.KhoHangs.Find(id);
            if (khohang == null)
            {
                return HttpNotFound();
            }
            ViewBag.SachID = new SelectList(db.Saches, "SachID", "TenSach", khohang.SachID);
            return View(khohang);
        }

        //
        // POST: /Admin/KhoHang/Edit/5

        [HttpPost]
        public ActionResult Edit(KhoHang khohang, int slOld)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khohang).State = EntityState.Modified;
                Sach s = db.Saches.Find(khohang.SachID);
                s.SoLuongTon = (s.SoLuongTon - slOld) + khohang.SoLuongNhap;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SachID = new SelectList(db.Saches, "SachID", "TenSach", khohang.SachID);
            return View(khohang);
        }

        //
        // GET: /Admin/KhoHang/Delete/5

        public ActionResult Delete(int id = 0)
        {
            KhoHang khohang = db.KhoHangs.Find(id);
            if (khohang == null)
            {
                return HttpNotFound();
            }
            return View(khohang);
        }

        //
        // POST: /Admin/KhoHang/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            KhoHang khohang = db.KhoHangs.Find(id);
            Sach s = db.Saches.Find(khohang.SachID);
            s.SoLuongTon = s.SoLuongTon - khohang.SoLuongNhap;
            db.KhoHangs.Remove(khohang);
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