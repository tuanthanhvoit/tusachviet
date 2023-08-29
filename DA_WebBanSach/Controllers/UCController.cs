using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;


namespace DA_WebBanSach.Controllers
{
    public class UCController : Controller
    {
        //
        // GET: /UC/
        SachDbContext db = new SachDbContext();

        public ActionResult MenuTop()
        {
            return PartialView();
        }

        public ActionResult TimKiem()
        {
            return PartialView();
        }

        public ActionResult MenuSach()
        {
            var model = db.ChungLoaiSaches.Include("LoaiSaches").ToList();
            return PartialView(model);
        }

        public ActionResult SlideTop()
        {
            var model = db.TinTucs.OrderByDescending(d => d.NgayViet).Take(5).ToList();
            return PartialView(model);
        }

        public ActionResult HoTro()
        {
            return PartialView();
        }

        public ActionResult Login()
        {
            return PartialView();
        }

        public ActionResult QuangCao()
        {
            return View();
        }

        public ActionResult SachXemNhieu()
        {
            var model = db.ChiTietDonHangs.GroupBy(c=>c.Sach).Select(g=>new {Sach=g.Key, Tong=g.Sum(c=>c.SoLuong)}).OrderByDescending(i=>i.Tong).Select(c=>c.Sach).Take(10).ToList();
            return PartialView(model);
        }

        public ActionResult RegisterFast()
        {
            return PartialView();
        }
    }
}
