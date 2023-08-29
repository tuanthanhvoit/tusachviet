using DA_WebBanSach.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Areas.QuanLy.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin,NhanVien,HoaDon,BanHang")]
    public class HomeController : Controller
    {
        //
        // GET: /QuanLy/Home/
        public ActionResult Index()
        {
            return View();
        }

    }
}
