using DA_WebBanSach.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [AreaAuthorizeAttribute(Roles="Admin")]
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
