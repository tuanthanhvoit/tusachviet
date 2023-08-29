using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Controllers
{
    public class TimKiemController : Controller
    {
        //
        // GET: /TimKiem/
        private SachDbContext db = new SachDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string search, string theoLoai = "byTenSach")
        {
            if (search != "")
            {
                ViewBag.tukhoa = search;
                if (theoLoai == "byTenSach")
                {
                    var sach = db.Saches.Where(s => s.TenSach.Contains(search)).ToList();
                    if (sach.Count > 0)
                    {
                        return View(sach);
                    }
                    else {
                        ViewBag.Error = "Không Tìm Thấy Sách Cần Tìm";
                        return View();
                    }
                }
                else
                {
                    var sach = db.ChiTietTacGias.Where(s => s.TacGia.TenTG == search).Select(s => s.Sach).ToList();
                    if (sach.Count > 0)
                    {
                        return View(sach);
                    }
                    else
                    {
                        ViewBag.Error = "Không Tìm Thấy Sách Có Tên Tác Giả Cần Tìm";
                        return View();
                    }
                }
            }
            else {
                ViewBag.Error = "";
                return View();
            }
        }
    }
}
