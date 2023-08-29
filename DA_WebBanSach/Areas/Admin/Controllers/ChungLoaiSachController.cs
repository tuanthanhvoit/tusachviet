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
    public class ChungLoaiSachController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/ChungLoaiSach/

        public ActionResult Index()
        {
            return View(db.ChungLoaiSaches.ToList());
        }

        //
        // GET: /Admin/ChungLoaiSach/Details/5

        public ActionResult Details(int id = 0)
        {
            ChungLoaiSach chungloaisach = db.ChungLoaiSaches.Find(id);
            if (chungloaisach == null)
            {
                return HttpNotFound();
            }
            return View(chungloaisach);
        }

        //
        // GET: /Admin/ChungLoaiSach/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/ChungLoaiSach/Create

        [HttpPost]
        public ActionResult Create(ChungLoaiSach chungloaisach)
        {
            if (ModelState.IsValid)
            {
                db.ChungLoaiSaches.Add(chungloaisach);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = chungloaisach.ChungLoaiSachID });
            }

            return View(chungloaisach);
        }

        //
        // GET: /Admin/ChungLoaiSach/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ChungLoaiSach chungloaisach = db.ChungLoaiSaches.Find(id);
            if (chungloaisach == null)
            {
                return HttpNotFound();
            }
            return View(chungloaisach);
        }

        //
        // POST: /Admin/ChungLoaiSach/Edit/5

        [HttpPost]
        public ActionResult Edit(ChungLoaiSach chungloaisach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chungloaisach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = chungloaisach.ChungLoaiSachID });
            }
            return View(chungloaisach);
        }

        //
        // GET: /Admin/ChungLoaiSach/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ChungLoaiSach chungloaisach = db.ChungLoaiSaches.Find(id);
            if (chungloaisach == null)
            {
                return HttpNotFound();
            }
            return View(chungloaisach);
        }

        //
        // POST: /Admin/ChungLoaiSach/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ChungLoaiSach chungloaisach = db.ChungLoaiSaches.Find(id);
            db.ChungLoaiSaches.Remove(chungloaisach);
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