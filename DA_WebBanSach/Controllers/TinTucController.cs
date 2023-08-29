using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Controllers
{
    public class TinTucController : Controller
    {
        //
        // GET: /TinTuc/
        private SachDbContext db = new SachDbContext();

        public ActionResult Index()
        {
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

            PhanTrang pi = PhanTrang.Get("TinTuc", 5);
            pi.RowCount = db.TinTucs.Count();
            pi.Navigate(pageNo);

            int startRow = pi.PageNo * pi.PageSize;
            var model = db.TinTucs.OrderByDescending(s => s.NgayViet).Skip(startRow).Take(pi.PageSize).ToList();
            return View(model);
        }

        public ActionResult ChiTiet(int id) {
            var model = db.TinTucs.Find(id);
            if (model==null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
    }
}
