using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;
using System.Web.Security;
using DA_WebBanSach.Filters;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin")]
    public class NguoiDungController : Controller
    {
        private SachDbContext db = new SachDbContext();
        private List<SelectListItem> gioitinh = new List<SelectListItem>(){
                new SelectListItem() {Text="Nam", Value="true"},
                new SelectListItem() {Text="Nữ", Value="false"}
        };
        //
        // GET: /Admin/NguoiDung/

        public ActionResult Index()
        {
            string[] username = Roles.GetUsersInRole("KhachHang");
            List<NguoiDung> model = new List<NguoiDung>();
            foreach (string item in username)
            {
                var ng = db.NguoiDungs.Where(u => u.UserName == item).ToList();
                model.AddRange(ng);
            }

            return View(model);
        }

        //
        // GET: /Admin/NguoiDung/Details/5

        public ActionResult Details(int id = 0)
        {
            NguoiDung nguoidung = db.NguoiDungs.Find(id);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            return View(nguoidung);
        }

        //
        // GET: /Admin/NguoiDung/Edit/5

        public ActionResult Edit(int id = 0)
        {
            
            NguoiDung nguoidung = db.NguoiDungs.Find(id);
            ViewBag.gioitinh = new SelectList(gioitinh, "Value", "Text", nguoidung.GioiTinh);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            if (!Roles.GetRolesForUser(nguoidung.UserName).Contains("KhachHang"))
            {
                return HttpNotFound();
            }
            return View(nguoidung);
        }

        //
        // POST: /Admin/NguoiDung/Edit/5

        [HttpPost]
        public ActionResult Edit(NguoiDung nguoidung)
        {
            ViewBag.gioitinh = new SelectList(gioitinh, "Value", "Text", nguoidung.GioiTinh);
            if (ModelState.IsValid)
            {
                db.Entry(nguoidung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = nguoidung.UserId });
            }
            return View(nguoidung);
        }

        //
        // GET: /Admin/NguoiDung/Delete/5

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}