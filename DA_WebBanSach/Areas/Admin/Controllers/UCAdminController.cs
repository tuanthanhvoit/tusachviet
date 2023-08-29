using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    public class UCAdminController : Controller
    {
        //
        // GET: /Admin/UCAdmin/

        public ActionResult Menu()
        {
            return PartialView();
        }

    }
}
