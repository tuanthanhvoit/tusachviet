using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Controllers
{
    public class PageErrorController : Controller
    {
        //
        // GET: /PageError/

        public ActionResult Error(string errorCode)
        {
            int code = 0;
            int.TryParse(errorCode, out code);
            switch (code) { 
                case 403:
                    return RedirectToAction("Error403");
                case 404:
                    return RedirectToAction("Error404");
                case 500:
                    return RedirectToAction("Error500");
                default:
                    return RedirectToAction("GeneralError");
            }
        }

        public ActionResult Error403()
        {
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }
        public ActionResult GeneralError()
        {
            return View();
        }
    }
}
