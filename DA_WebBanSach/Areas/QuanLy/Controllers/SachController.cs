using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;
using DA_WebBanSach.Filters;
using System.IO;

namespace DA_WebBanSach.Areas.QuanLy.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin,NhanVien")]
    public class SachController : Controller
    {
        private SachDbContext db = new SachDbContext();

        private List<SelectListItem> loaibia = new List<SelectListItem>(){
                new SelectListItem() {Text="Bìa cứng", Value="true"},
                new SelectListItem() {Text="Bìa mềm", Value="false"}
        };
        //
        // GET: /Admin/Sach/

        public ActionResult Index()
        {
            ViewBag.LoaiBia = new SelectList(loaibia, "Value", "Text");
            var saches = db.Saches.Include(s => s.NhaXuatBan);
            return View(saches.ToList());
        }

        //
        // GET: /Admin/Sach/Details/5

        public ActionResult Details(int id = 0)
        {
            ViewBag.LoaiBia = new SelectList(loaibia, "Value", "Text");
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        //
        // GET: /Admin/Sach/Create

        public ActionResult Create()
        {
            ViewBag.LoaiBia = new SelectList(loaibia, "Value", "Text");
            ViewBag.NhaXuatBanID = new SelectList(db.NhaXuatBans, "NhaXuatBanID", "TenNXB");
            return View();
        }

        //
        // POST: /Admin/Sach/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Sach sach, HttpPostedFileBase myfile)
        {
                sach.SoLuongTon = 0;

                if (myfile != null)
                {
                    if (myfile.FileName.Contains(".jpg") || myfile.FileName.Contains(".png"))
                    {
                        var fileName = "";
                        if (myfile.ContentLength > 0 && myfile.ContentLength < 1024 * 1024 * 2)
                        {
                                fileName = Convert.ToDateTime(DateTime.Now).ToString() + Path.GetFileName(myfile.FileName);
                                fileName = fileName.Replace("/", "");
                                fileName = fileName.Replace(":", "");
                                fileName = fileName.Replace(" ", "-");
                                var path = Path.Combine(Server.MapPath("~/Images/Sach/"), fileName);
                                myfile.SaveAs(path);
                                sach.AnhBia = fileName;
                                db.Saches.Add(sach);
                                db.SaveChanges();
                                return RedirectToAction("Details", new { id = sach.SachID });
                        }
                        else
                        {
                            ViewData["kichthuoc"] = "Upload ảnh dưới 2MB";
                        }
                    }
                    else
                    {
                        ViewData["loaianh"] = "File ảnh không hợp lệ";
                    }
                }
                else
                {
                    ViewData["hinh"] = "Chưa chọn hình ảnh";
                }
                
            ViewBag.LoaiBia = new SelectList(loaibia, "Value", "Text", sach.LoaiBia);
            ViewBag.NhaXuatBanID = new SelectList(db.NhaXuatBans, "NhaXuatBanID", "TenNXB", sach.NhaXuatBanID);
            return View(sach);
        }

        //
        // GET: /Admin/Sach/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoaiBia = new SelectList(loaibia, "Value", "Text", sach.LoaiBia);
            ViewBag.NhaXuatBanID = new SelectList(db.NhaXuatBans, "NhaXuatBanID", "TenNXB", sach.NhaXuatBanID);
            return View(sach);
        }

        //
        // POST: /Admin/Sach/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Sach sach, HttpPostedFileBase myfile)
        {
            if (ModelState.IsValid)
            {
                if (myfile != null)
                {
                    if (myfile.FileName.Contains(".jpg") || myfile.FileName.Contains(".png"))
                    {
                        var fileName = "";
                        if (myfile.ContentLength > 0 && myfile.ContentLength < 1024 * 1024 * 2)
                        {
                            fileName = Convert.ToDateTime(DateTime.Now).ToString() + Path.GetFileName(myfile.FileName);
                            fileName = fileName.Replace("/", "");
                            fileName = fileName.Replace(":", "");
                            fileName = fileName.Replace(" ", "-");
                            var path = Path.Combine(Server.MapPath("~/Images/Sach/"), fileName);
                            myfile.SaveAs(path);
                            sach.AnhBia = fileName;
                        }
                        else
                        {
                            ViewData["kichthuoc"] = "Upload ảnh dưới 2MB";
                        }
                    }
                    else
                    {
                        ViewData["loaianh"] = "File ảnh không hợp lệ";
                    }
                }
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = sach.SachID });
            }
            ViewBag.LoaiBia = new SelectList(loaibia, "Value", "Text", sach.LoaiBia);
            ViewBag.NhaXuatBanID = new SelectList(db.NhaXuatBans, "NhaXuatBanID", "TenNXB", sach.NhaXuatBanID);
            return View(sach);
        }

        //
        // GET: /Admin/Sach/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        //
        // POST: /Admin/Sach/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sach sach = db.Saches.Find(id);
            db.Saches.Remove(sach);
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