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

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin")]
    public class TinTucController : Controller
    {
        private SachDbContext db = new SachDbContext();

        //
        // GET: /Admin/TinTuc/

        public ActionResult Index()
        {
            var tintucs = db.TinTucs.Include(t => t.NguoiDung);
            return View(tintucs.ToList());
        }

        //
        // GET: /Admin/TinTuc/Details/5

        public ActionResult Details(int id = 0)
        {
            TinTuc tintuc = db.TinTucs.Find(id);
            if (tintuc == null)
            {
                return HttpNotFound();
            }
            return View(tintuc);
        }

        //
        // GET: /Admin/TinTuc/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.NguoiDungs, "UserId", "UserName");
            return View();
        }

        //
        // POST: /Admin/TinTuc/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TinTuc tintuc, HttpPostedFileBase myfile)
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
                                var path = Path.Combine(Server.MapPath("~/Images/TinTuc/"), fileName);
                                myfile.SaveAs(path);
                                tintuc.UrlHinh = fileName;
                                db.TinTucs.Add(tintuc);
                                db.SaveChanges();
                                return RedirectToAction("Details", new { id = tintuc.TinTucID });
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

            ViewBag.UserId = new SelectList(db.NguoiDungs, "UserId", "UserName", tintuc.UserId);
            return View(tintuc);
        }

        //
        // GET: /Admin/TinTuc/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TinTuc tintuc = db.TinTucs.Find(id);
            if (tintuc == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.NguoiDungs, "UserId", "UserName", tintuc.UserId);
            return View(tintuc);
        }

        //
        // POST: /Admin/TinTuc/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TinTuc tintuc, HttpPostedFileBase myfile)
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
                            var path = Path.Combine(Server.MapPath("~/Images/TinTuc/"), fileName);
                            myfile.SaveAs(path);
                            tintuc.UrlHinh = fileName;
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
                db.Entry(tintuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = tintuc.TinTucID });
            }

            ViewBag.UserId = new SelectList(db.NguoiDungs, "UserId", "UserName", tintuc.UserId);
            return View(tintuc);
        }

        //
        // GET: /Admin/TinTuc/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TinTuc tintuc = db.TinTucs.Find(id);
            if (tintuc == null)
            {
                return HttpNotFound();
            }
            return View(tintuc);
        }

        //
        // POST: /Admin/TinTuc/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TinTuc tintuc = db.TinTucs.Find(id);
            db.TinTucs.Remove(tintuc);
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