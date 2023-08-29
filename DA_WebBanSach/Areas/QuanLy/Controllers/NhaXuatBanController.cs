using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;
using DA_WebBanSach.Filters;

namespace DA_WebBanSach.Areas.QuanLy.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin,NhanVien")]
    public class NhaXuatBanController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/NhaXuatBan/

        public ActionResult Index()
        {
            return View(db.NhaXuatBans.ToList());
        }

        //
        // GET: /Admin/NhaXuatBan/Details/5

        public ActionResult Details(int id = 0)
        {
            NhaXuatBan nhaxuatban = db.NhaXuatBans.Find(id);
            if (nhaxuatban == null)
            {
                return HttpNotFound();
            }
            return View(nhaxuatban);
        }

        //
        // GET: /Admin/NhaXuatBan/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/NhaXuatBan/Create

        [HttpPost]
        public ActionResult Create(NhaXuatBan nhaxuatban)
        {
            if (ModelState.IsValid)
            {
                db.NhaXuatBans.Add(nhaxuatban);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=nhaxuatban.NhaXuatBanID});
            }

            return View(nhaxuatban);
        }

        //
        // GET: /Admin/NhaXuatBan/Edit/5

        public ActionResult Edit(int id = 0)
        {
            NhaXuatBan nhaxuatban = db.NhaXuatBans.Find(id);
            if (nhaxuatban == null)
            {
                return HttpNotFound();
            }
            return View(nhaxuatban);
        }

        //
        // POST: /Admin/NhaXuatBan/Edit/5

        [HttpPost]
        public ActionResult Edit(NhaXuatBan nhaxuatban)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaxuatban).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = nhaxuatban.NhaXuatBanID });
            }
            return View(nhaxuatban);
        }

        //
        // GET: /Admin/NhaXuatBan/Delete/5

        public ActionResult Delete(int id = 0)
        {
            NhaXuatBan nhaxuatban = db.NhaXuatBans.Find(id);
            if (nhaxuatban == null)
            {
                return HttpNotFound();
            }
            return View(nhaxuatban);
        }

        //
        // POST: /Admin/NhaXuatBan/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NhaXuatBan nhaxuatban = db.NhaXuatBans.Find(id);
            db.NhaXuatBans.Remove(nhaxuatban);
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