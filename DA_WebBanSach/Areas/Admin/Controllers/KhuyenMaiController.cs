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
    public class KhuyenMaiController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/KhuyenMai/

        public ActionResult Index()
        {
            return View(db.KhuyenMais.ToList());
        }

        //
        // GET: /Admin/KhuyenMai/Details/5

        public ActionResult Details(int id = 0)
        {
            KhuyenMai khuyenmai = db.KhuyenMais.Find(id);
            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenmai);
        }

        //
        // GET: /Admin/KhuyenMai/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/KhuyenMai/Create

        [HttpPost]
        public ActionResult Create(KhuyenMai khuyenmai)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMais.Add(khuyenmai);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=khuyenmai.KhuyenMaiID});
            }

            return View(khuyenmai);
        }

        //
        // GET: /Admin/KhuyenMai/Edit/5

        public ActionResult Edit(int id = 0)
        {
            KhuyenMai khuyenmai = db.KhuyenMais.Find(id);
            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenmai);
        }

        //
        // POST: /Admin/KhuyenMai/Edit/5

        [HttpPost]
        public ActionResult Edit(KhuyenMai khuyenmai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenmai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = khuyenmai.KhuyenMaiID });
            }
            return View(khuyenmai);
        }

        //
        // GET: /Admin/KhuyenMai/Delete/5

        public ActionResult Delete(int id = 0)
        {
            KhuyenMai khuyenmai = db.KhuyenMais.Find(id);
            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenmai);
        }

        //
        // POST: /Admin/KhuyenMai/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            KhuyenMai khuyenmai = db.KhuyenMais.Find(id);
            db.KhuyenMais.Remove(khuyenmai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ThemSachKhuyenMai(int Id) {
            var model = db.KhuyenMais.Where(k => k.KhuyenMaiID == Id).Single();

            var sach = db.Saches.Where(s => !db.ChiTietKhuyenMais.Where(z => z.KhuyenMaiID == Id).Any(y => y.SachID == s.SachID)).ToList();

            ViewBag.Saches = sach;
            return View(model);
        }

        [HttpPost]
        public ActionResult ThemSachKhuyenMai(KhuyenMai model)
        {
            String[] names = Request.Form.AllKeys;
            foreach (var name in names)
            {
                if (name.StartsWith("qty"))
                {
                    int id = int.Parse(name.Substring(3));
                    ChiTietKhuyenMai ct = new ChiTietKhuyenMai() { 
                        KhuyenMaiID = model.KhuyenMaiID,
                        SachID = id
                    };
                    db.ChiTietKhuyenMais.Add(ct);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "Lỗi Khi Thêm Sách Vào Khuyến Mãi";
                return View();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}