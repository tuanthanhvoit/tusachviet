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
    public class TacGiaController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/TacGia/

        public ActionResult Index()
        {
            return View(db.TacGias.ToList());
        }

        //
        // GET: /Admin/TacGia/Details/5

        public ActionResult Details(int id = 0)
        {
            TacGia tacgia = db.TacGias.Find(id);
            if (tacgia == null)
            {
                return HttpNotFound();
            }
            return View(tacgia);
        }

        //
        // GET: /Admin/TacGia/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/TacGia/Create

        [HttpPost]
        public ActionResult Create(TacGia tacgia)
        {
            if (ModelState.IsValid)
            {
                db.TacGias.Add(tacgia);
                db.SaveChanges();
                return RedirectToAction("Details", new { id= tacgia.TacGiaID});
            }

            return View(tacgia);
        }

        //
        // GET: /Admin/TacGia/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TacGia tacgia = db.TacGias.Find(id);
            if (tacgia == null)
            {
                return HttpNotFound();
            }
            return View(tacgia);
        }

        //
        // POST: /Admin/TacGia/Edit/5

        [HttpPost]
        public ActionResult Edit(TacGia tacgia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tacgia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = tacgia.TacGiaID });
            }
            return View(tacgia);
        }

        //
        // GET: /Admin/TacGia/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TacGia tacgia = db.TacGias.Find(id);
            if (tacgia == null)
            {
                return HttpNotFound();
            }
            return View(tacgia);
        }

        //
        // POST: /Admin/TacGia/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TacGia tacgia = db.TacGias.Find(id);
            db.TacGias.Remove(tacgia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        //  GET: /Admin/LoaiSach/ThemSachTacGia/Id
        public ActionResult ThemSachTacGia(int Id)
        {
            var model = db.TacGias.Where(k => k.TacGiaID == Id).Single();

            var sach = db.Saches.Where(s => !db.ChiTietTacGias.Where(z => z.TacGiaID == Id).Any(y => y.SachID == s.SachID)).ToList();

            ViewBag.Saches = sach;
            return View(model);
        }


        //
        //  POST
        [HttpPost]
        public ActionResult ThemSachTacGia(TacGia model)
        {
            String[] names = Request.Form.AllKeys;
            foreach (var name in names)
            {
                if (name.StartsWith("qty"))
                {
                    int id = int.Parse(name.Substring(3));
                    ChiTietTacGia ct = new ChiTietTacGia()
                    {
                        TacGiaID = model.TacGiaID,
                        SachID = id
                    };
                    db.ChiTietTacGias.Add(ct);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "Lỗi Khi Thêm Sách Vào Loại Sách";
                return RedirectToAction("ThemSachTacGia", new { id = model.TacGiaID });
            }

            return RedirectToAction("ThemSachTacGia", new { id = model.TacGiaID });
        }

        public ActionResult XoaSachTacGia(int Id)
        {
            var model = db.TacGias.Where(k => k.TacGiaID == Id).Single();

            var sach = db.ChiTietTacGias.Where(s => s.TacGiaID == Id).Select(s => s.Sach).ToList();

            ViewBag.Saches = sach;
            return View(model);
        }

        [HttpPost]
        public ActionResult XoaSachTacGia(TacGia model)
        {
            var sach = db.ChiTietTacGias.Where(s => s.TacGiaID == model.TacGiaID).Select(s => s.Sach).ToList();
            String[] names = Request.Form.AllKeys;
            foreach (var name in names)
            {
                if (name.StartsWith("chk"))
                {
                    int id = int.Parse(name.Substring(3));
                    ChiTietTacGia ct = db.ChiTietTacGias.Where(s => s.SachID == id && s.TacGiaID == model.TacGiaID).Single();
                    db.ChiTietTacGias.Remove(ct);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "Lỗi Khi Xóa Sách Trong Tác Giả";
                ViewBag.Saches = sach;
                return View(model);
            }
            ViewBag.Saches = sach;
            return RedirectToAction("XoaSachTacGia", new { id = model.TacGiaID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}