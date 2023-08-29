using DA_WebBanSach.Filters;
using DA_WebBanSach.Models;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [InitializeSimpleMembership]
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index(string ReturnUrl)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin") == true)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost,ActionName("Index")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                WebSecurity.Logout();
            }

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }
            else {
                WebSecurity.Logout();
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Tên Đăng Nhập Hoặc Mật Khẩu Không Chính Xác");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Tên Đăng Nhập Đã Tồn Tại";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Email đã được xử dụng";

                case MembershipCreateStatus.InvalidPassword:
                    return "Mật Khẩu Không Hợp Lệ";

                case MembershipCreateStatus.InvalidEmail:
                    return "Email Không Hợp lệ";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Tên người dùng được cung cấp không hợp lệ. Hãy kiểm tra các giá trị và thử lại.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }


}
