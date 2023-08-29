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
    public class LoaiSachController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/LoaiSach/

        public ActionResult Index()
        {
            var loaisaches = db.LoaiSaches.Include(l => l.ChungLoaiSach);
            return View(loaisaches.ToList());
        }

        //
        // GET: /Admin/LoaiSach/Details/5

        public ActionResult Details(int id = 0)
        {
            LoaiSach loaisach = db.LoaiSaches.Find(id);
            if (loaisach == null)
            {
                return HttpNotFound();
            }
            return View(loaisach);
        }

        //
        // GET: /Admin/LoaiSach/Create

        public ActionResult Create()
        {
            ViewBag.ChungLoaiSachID = new SelectList(db.ChungLoaiSaches, "ChungLoaiSachID", "TenChungLoai");
            return View();
        }

        //
        // POST: /Admin/LoaiSach/Create

        [HttpPost]
        public ActionResult Create(LoaiSach loaisach)
        {
            if (ModelState.IsValid)
            {
                db.LoaiSaches.Add(loaisach);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=loaisach.LoaiSachID});
            }

            ViewBag.ChungLoaiSachID = new SelectList(db.ChungLoaiSaches, "ChungLoaiSachID", "TenChungLoai", loaisach.ChungLoaiSachID);
            return View(loaisach);
        }

        //
        // GET: /Admin/LoaiSach/Edit/5

        public ActionResult Edit(int id = 0)
        {
            LoaiSach loaisach = db.LoaiSaches.Find(id);
            if (loaisach == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChungLoaiSachID = new SelectList(db.ChungLoaiSaches, "ChungLoaiSachID", "TenChungLoai", loaisach.ChungLoaiSachID);
            return View(loaisach);
        }

        //
        // POST: /Admin/LoaiSach/Edit/5

        [HttpPost]
        public ActionResult Edit(LoaiSach loaisach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaisach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = loaisach.LoaiSachID });
            }
            ViewBag.ChungLoaiSachID = new SelectList(db.ChungLoaiSaches, "ChungLoaiSachID", "TenChungLoai", loaisach.ChungLoaiSachID);
            return View(loaisach);
        }

        //
        // GET: /Admin/LoaiSach/Delete/5

        public ActionResult Delete(int id = 0)
        {
            LoaiSach loaisach = db.LoaiSaches.Find(id);
            if (loaisach == null)
            {
                return HttpNotFound();
            }
            return View(loaisach);
        }

        //
        // POST: /Admin/LoaiSach/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiSach loaisach = db.LoaiSaches.Find(id);
            db.LoaiSaches.Remove(loaisach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        //  GET: /Admin/LoaiSach/ThemSachLoaiSach/Id
        public ActionResult ThemSachLoaiSach(int Id)
        {
            var model = db.LoaiSaches.Where(k => k.LoaiSachID == Id).Single();

            var sach = db.Saches.Where(s => !db.ChiTietLoaiSaches.Where(z => z.LoaiSachID == Id).Any(y => y.SachID == s.SachID)).ToList();

            ViewBag.Saches = sach;
            return View(model);
        }


        //
        //  POST
        [HttpPost]
        public ActionResult ThemSachLoaiSach(LoaiSach model)
        {
            String[] names = Request.Form.AllKeys;
            foreach (var name in names)
            {
                if (name.StartsWith("qty"))
                {
                    int id = int.Parse(name.Substring(3));
                    ChiTietLoaiSach ct = new ChiTietLoaiSach()
                    {
                        LoaiSachID = model.LoaiSachID,
                        SachID = id
                    };
                    db.ChiTietLoaiSaches.Add(ct);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "Lỗi Khi Thêm Sách Vào Loại Sách";
                return RedirectToAction("ThemSachLoaiSach", new { id=model.LoaiSachID});
            }

            return RedirectToAction("ThemSachLoaiSach", new { id = model.LoaiSachID });
        }

        public ActionResult XoaSachLoaiSach(int Id)
        {
            var model = db.LoaiSaches.Where(k => k.LoaiSachID == Id).Single();

            var sach = db.ChiTietLoaiSaches.Where(s => s.LoaiSachID == Id).Select(s => s.Sach).ToList();
            ViewBag.Saches = sach;
            return View(model);
        }

        [HttpPost]
        public ActionResult XoaSachLoaiSach(LoaiSach model)
        {
            var sach = db.ChiTietLoaiSaches.Where(s => s.LoaiSachID == model.LoaiSachID).Select(s => s.Sach).ToList();
            String[] names = Request.Form.AllKeys;
            foreach (var name in names)
            {
                if (name.StartsWith("chk"))
                {
                    int id = int.Parse(name.Substring(3));
                    ChiTietLoaiSach ct = db.ChiTietLoaiSaches.Where(s => s.SachID == id && s.LoaiSachID == model.LoaiSachID).Single();
                    db.ChiTietLoaiSaches.Remove(ct);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "Lỗi Khi Xóa Sách Trong Loại Sách";
                ViewBag.Saches = sach;
                return View(model);
            }
            ViewBag.Saches = sach;
            return RedirectToAction("XoaSachLoaiSach", new { id = model.LoaiSachID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}