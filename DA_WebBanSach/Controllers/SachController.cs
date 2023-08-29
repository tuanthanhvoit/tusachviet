using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Controllers
{
    public class SachController : Controller
    {
        //
        // GET: /Sach/
        SachDbContext db = new SachDbContext();

        public ActionResult Index(int id=1)
        {
            var sach = db.Saches.Where(s => s.SachID == id).Select(s => new SachKM { Sach=s}).Single();
            SachKM sKm = new SachKM();

            sach = sKm.capnhatKM(sach);

            var ctls = sach.Sach.ChiTietLoaiSaches.ToList();

            List<Sach> lsach = new List<Sach>();
            foreach (var item in ctls)
            {
                var st = db.ChiTietLoaiSaches.Where(t => t.LoaiSachID == item.LoaiSachID).Select(d => d.Sach).ToList();
                lsach.AddRange(st);
            }

            int count = lsach.Count;
            Random r = new Random();
            ViewBag.listsach = lsach.Skip(r.Next(0,count)).Take(4);

            return View(sach);
        }

    }
}
