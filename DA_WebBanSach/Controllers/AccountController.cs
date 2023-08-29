using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using DA_WebBanSach.Filters;
using DA_WebBanSach.Models;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;


namespace DA_WebBanSach.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {

        private SachDbContext db = new SachDbContext();
        private List<SelectListItem> hinhthuc = new List<SelectListItem>() { 
            new SelectListItem(){Text="Thu Tiền Tại Nhà", Value="0"},
            new SelectListItem(){Text="Ngân Lượng", Value="1"},
        };

        public ActionResult ChiTietDonHang(int Id) {
            DonHang dh = db.DonHangs.Find(Id);
            if (dh.UserId != WebSecurity.GetUserId(User.Identity.Name))
            {
                return HttpNotFound();
            }

            if (dh==null)
            {
                return HttpNotFound();
            }
            ViewBag.HinhThucThanhToan = new SelectList(hinhthuc, "Value", "Text");
            return View(dh);
        }

        [HttpPost]
        public ActionResult ChiTietDonHang(string MaDonHang)
        {
            int ma = int.Parse(MaDonHang);
            DonHang dh = db.DonHangs.Find(ma);
            String return_url = "http://tusachviet.org/thanhtoan/thanhtoanthanhcong/";// Địa chỉ trả về 
            String transaction_info = "Thanh toán mua hàng tại tusachviet.org";//Thông tin giao dịch
            String order_code = dh.DonHangID.ToString();//Mã hoa don 
            String receiver = "tuanthanhvo.1989@gmail.com";//Tài khoản nhận tiền 
            String price = dh.TongTien.ToString();

            NganLuong nl = new NganLuong();
            string url = nl.buildCheckoutUrl(return_url, receiver, transaction_info, order_code, price);
            return Redirect(url);
        }

        public ActionResult ThongTinCaNhan()
        {
            var user = db.NguoiDungs.Where(u => u.UserName == User.Identity.Name).Single();
            return View(user);
        }

        public ActionResult SuaThongTinCaNhan(string Id)
        {
            try
            {
                int UserID = WebSecurity.GetUserId(User.Identity.Name);
                var kh = db.NguoiDungs.Where(k => k.UserId == UserID).Single();
                if (kh.UserId == int.Parse(Id))
                {
                    return View(kh);
                }
                else
                {
                    var _kh = db.NguoiDungs.Where(k => k.UserId == UserID).Single();
                    return View(_kh);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ThongTinCaNhan");
            }
        }
        
        [HttpPost]
        public ActionResult SuaThongTinCaNhan(NguoiDung model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NguoiDung nd = db.NguoiDungs.Find(model.UserId);
                    nd.DiaChi = model.DiaChi;
                    nd.Quan = model.Quan;
                    nd.ThanhPho = model.ThanhPho;
                    nd.DienThoai = model.DienThoai;
                    nd.GioiTinh = model.GioiTinh;
                    nd.HoTen = model.HoTen;
                    nd.NgaySinh = model.NgaySinh;
                    db.SaveChanges();
                    return RedirectToAction("ThongTinCaNhan");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Không Sửa Được Thông Tin Cá Nhân";
            }
            return View(model);
        }

        public ActionResult DoiMatKhau()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoiMatKhau(String OldPassword, String NewPassword, String ReNewPassword)
        {
            if (NewPassword != ReNewPassword)
            {
                ViewBag.Error = "Nhập lại mật khẩu mới không khớp";
            }
            else
            {
                try
                {
                    string username = User.Identity.Name;
                    MembershipUser user = Membership.GetUser(username);
                    if (user.ChangePassword(OldPassword, NewPassword))
                    {
                        ViewBag.Error = "Đổi mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Sai mật khẩu cũ";
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Login", "Account", new { ReturnUrl = "/Account/DoiMatKhau" });
                }
            }
            return View();
        }

        public ActionResult SachDaMua()
        {
            int iduser;
            try
            {
                iduser = WebSecurity.GetUserId(User.Identity.Name);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/Account/SachDaMua" });
            }

            var mySachs = db.ChiTietDonHangs.Where(o => o.DonHang.NguoiDung.UserId == iduser).Select(s => s.Sach).Distinct().ToList();
            return View(mySachs);
        }

        public ActionResult XemDonHang()
        {
            int iduser;
            try
            {
                iduser = WebSecurity.GetUserId(User.Identity.Name);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/Account/XemDonHang" });
            }

            var myOrders = db.DonHangs
                .Where(o => o.UserId == iduser)
                .OrderByDescending(o => o.NgayDatHang);
            return View(myOrders);
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Tên Đăng Nhập Hoặc Mật Khẩu Không Chính Xác");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            GioHang.Cart.Clear();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/RegisterFast
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterFast(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                using (SachDbContext db = new SachDbContext())
                {
                    NguoiDung email = db.NguoiDungs.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                    if (email == null)
                    {
                        try
                        {
                            string confirmToken = WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new
                            {
                                Email = model.Email,
                                GioiTinh = true,
                                NgaySinh = "1990/01/01"
                            }, requireConfirmationToken: true);

                            KhachHang nv = new KhachHang
                            {
                                UserId = WebSecurity.GetUserId(model.UserName),
                                TongTien = 0
                            };

                            db.KhachHangs.Add(nv);
                            db.SaveChanges();
                            Roles.AddUserToRole(model.UserName, "KhachHang");
                            SentMail.Send("admin@tusachviet.org", model.Email, "Kích hoạt tài khoản tại tusachviet.org", Request.Url.Authority + "/Account/Active/" + confirmToken);
                            return View("DangKiThanhCong");
                        }
                        catch (MembershipCreateUserException e)
                        {
                            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                        }
                    }
                    else 
                    {
                        ModelState.AddModelError("Email", "Email Đã Tồn Tại");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register","Account");
        }

        [AllowAnonymous]
        public ActionResult DangKiThanhCong() 
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Active(string Id)
        {
            if (Id != "")
            {
                if (WebSecurity.ConfirmAccount(Id))
                {
                    ViewBag.Error = "Kích hoạt tài khoản thành công";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Kích hoạt tài khoản không thành công xin kiểm tra lại đường dẫn kích hoạt";
                    return View();
                }
            }
            else {
                ViewBag.Error = "Vui Lòng Kiểm Tra Lại Link Kích Hoạt";
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult ResendActive() 
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResendActive(ConfirmModel model)
        {
            try
            {
                var user = db.NguoiDungs.Where(m => m.Email == model.Email).Single();
                string confirmToken = db.webpages_Membership.Where(f => f.UserId == user.UserId).Select(f => f.ConfirmationToken).Single();
                SentMail.Send("admin@tusachviet.org", model.Email, "Mã Kích Hoạt Tài Khoản Tại TuSachViet.org", Request.Url.Authority + "/Account/Active/" + confirmToken);
                ViewBag.Error = "Link Kích Hoạt Đã Được Gửi Lại Vui Lòng Kiểm Tra Lại Email";
            }
            catch (Exception)
            {
                ViewBag.Error = "Không Tìm Thấy Địa Chỉ Email Của Bạn Vui Lòng Nhập Lại Địa Chỉ Email";
            }
            
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModelFull model)
        {
            if (ModelState.IsValid)
            {
                using (SachDbContext db = new SachDbContext())
                {
                    NguoiDung email = db.NguoiDungs.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                    if (email == null)
                    {
                        try
                        {
                            string confirmToken = WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new
                            {
                                Email = model.Email,
                                HoTen = model.HoTen,
                                GioiTinh = model.GioiTinh,
                                NgaySinh = model.NgaySinh,
                                DienThoai = model.DienThoai,
                                DiaChi = model.DiaChi,
                                Quan = model.Quan,
                                ThanhPho = model.ThanhPho
                            }, requireConfirmationToken: true);


                            KhachHang kh = new KhachHang
                            {
                                UserId = WebSecurity.GetUserId(model.UserName),
                                TongTien = 0
                            };

                            db.KhachHangs.Add(kh);
                            db.SaveChanges();
                            Roles.AddUserToRole(model.UserName, "KhachHang");
                            SentMail.Send("admin@tusachviet.org", model.Email, "Kích hoạt tài khoản tại TuSachViet.org", Request.Url.Authority + "/Account/Active/" + confirmToken);

                            return View("DangKiThanhCong");
                        }
                        catch (MembershipCreateUserException e)
                        {
                            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Email Đã Tồn Tại");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            else {
                return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult QuenMatKhau()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult QuenMatKhau(ConfirmModel model)
        {
            try
            {
                var user = db.NguoiDungs.Where(m => m.Email == model.Email).Single();

                string pwToken = WebSecurity.GeneratePasswordResetToken(user.UserName);

                SentMail.Send("admin@tusachviet.org", model.Email, "Lấy Lại Mật Khẩu Tại tusachviet.org", user.UserName, Request.Url.Authority + "/Account/ResetPassword/" + pwToken);
                ViewBag.Error = "Mật Khẩu Đã Được Gửi Vui Lòng Kiểm Tra Lại Email";
            }
            catch (Exception)
            {
                ViewBag.Error = "Không Tìm Thấy Địa Chỉ Email Của Bạn Vui Lòng Nhập Lại Địa Chỉ Email";
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string Id)
        {
            if (Id!="")
            {
                ResetPasswordModel m = new ResetPasswordModel
                {
                    ChuoiConfirm = Id
                };
                return View(m);
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "Mật Khẩu nhập lại không chính xác";
                return View();
            }
            else {
                WebSecurity.ResetPassword(model.ChuoiConfirm, model.NewPassword);
                return RedirectToAction("Login", "Account");
            }
            
        }
        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    NguoiDung user = db.NguoiDungs.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.NguoiDungs.Add(new NguoiDung { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
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
