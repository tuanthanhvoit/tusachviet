using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Controllers
{
    public class LoaiSachController : Controller
    {
        //
        // GET: /LoaiSach/
        SachDbContext db = new SachDbContext();
        public ActionResult Index(int id=0)
        {
            ViewBag.LoaiSach = db.LoaiSaches.Where(ls => ls.LoaiSachID == id).Single();

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

            PhanTrang pi = PhanTrang.Get("LoaiSach", 10);
            var sachAll = db.ChiTietLoaiSaches.Where(s => s.LoaiSachID == id).Select(s => new SachKM { Sach=s.Sach}).ToList();
            pi.RowCount = sachAll.Count;
            pi.Navigate(pageNo);

            int startRow = pi.PageNo * pi.PageSize;
            SachKM sa = new SachKM();
            sachAll = sa.capnhatKM(sachAll);
            var sach = sachAll.Skip(startRow).Take(pi.PageSize);
            
            return View(sach);
        }

    }
}
