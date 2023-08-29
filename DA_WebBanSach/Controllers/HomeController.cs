using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DA_WebBanSach.Controllers
{
    public class HomeController : Controller
    {
        SachDbContext db = new SachDbContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Trang Chủ";

            int pageNo = 0;
            foreach (var key in Request.Form.AllKeys)
            {
                if (key.StartsWith("pageNo"))
                {
                    pageNo = int.Parse(key.Substring(7));
                    break;
                }
                else if (key.StartsWith("dentrang"))
                {
                    try
                    {
                        pageNo = int.Parse(Request.Form["pageHT"]) - 1;
                    }
                    catch (Exception)
                    {
                        pageNo = 0;
                    }   
                    break;
                }
            }

            PhanTrang pi = PhanTrang.Get("Sach", 10);
            pi.RowCount = db.Saches.Count();
            pi.Navigate(pageNo);
            
            int startRow = pi.PageNo * pi.PageSize;
            var model = db.Saches.OrderByDescending(s => s.NgayXuatBan).Select(d => new SachKM()
                {
                    Sach = d
                }).Skip(startRow).Take(pi.PageSize).ToList();
            SachKM sa = new SachKM();
            model = sa.capnhatKM(model);
            return View(model);
        }

        public ActionResult LienHe()
        {
            ViewBag.Message = "Trang Liên Hệ";

            return View();
        }

        public ActionResult GioiThieu()
        {
            ViewBag.Message = "Trang Giới Thiệu.";

            return View();
        }


    }
}
