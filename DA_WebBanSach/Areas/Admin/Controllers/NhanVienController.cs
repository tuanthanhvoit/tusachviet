using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_WebBanSach.Models;
using System.Web.Security;
using WebMatrix.WebData;
using DA_WebBanSach.Filters;

namespace DA_WebBanSach.Areas.Admin.Controllers
{
    [AreaAuthorizeAttribute(Roles = "Admin")]
    public class NhanVienController : Controller
    {
        private SachDbContext db = new SachDbContext();
        private List<SelectListItem> gioitinh = new List<SelectListItem>(){
                new SelectListItem() {Text="Nam", Value="true"},
                new SelectListItem() {Text="Nữ", Value="false"}
        };
        //
        // GET: /Admin/NhanVien/

        public ActionResult Index()
        {
            string[] kh = Roles.GetUsersInRole("KhachHang");
            var nv = db.NguoiDungs.Cast<NguoiDung>().Where(u => !kh.Contains(u.UserName)).ToList();
            return View(nv);
        }

        public ActionResult Create()
        {
            ViewBag.Roles = Roles.GetAllRoles();
            ViewBag.gioitinh = new SelectList(gioitinh, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Create(NhanVienModel model, DateTime ngayvaolam)
        {
            ViewBag.gioitinh = new SelectList(gioitinh, "Value", "Text", model.GioiTinh);
            if (ModelState.IsValid)
            {
                using (SachDbContext db = new SachDbContext())
                {
                    NguoiDung email = db.NguoiDungs.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                    if (email == null)
                    {
                        try
                        {

                            WebSecurity.CreateUserAndAccount(model.UserName, model.UserName, propertyValues: new
                            {
                                Email = model.Email,
                                HoTen = model.HoTen,
                                GioiTinh = model.GioiTinh,
                                NgaySinh = model.NgaySinh,
                                DienThoai = model.DienThoai,
                                DiaChi = model.DiaChi,
                                Quan = model.Quan,
                                ThanhPho = model.ThanhPho
                            });

                            NhanVien nv = new NhanVien
                            {
                                UserId = WebSecurity.GetUserId(model.UserName),
                                NgayVaoLam = ngayvaolam
                            };

                            db.NhanViens.Add(nv);
                            db.SaveChanges();

                            String[] oldRole = Roles.GetRolesForUser(model.UserName);
                            if (oldRole.Length > 0)
                            {
                                Roles.RemoveUserFromRoles(model.UserName, oldRole);
                            }

                            foreach (String role in Request.Form.Keys)
                            {
                                if (role.StartsWith("chk"))
                                {
                                    String rolename = role.Substring(3);
                                    Roles.AddUserToRole(model.UserName, rolename);
                                }
                            }
                            ViewBag.Roles = Roles.GetAllRoles();
                            return RedirectToAction("Details", new { id = nv.UserId });
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
                ViewBag.Roles = Roles.GetAllRoles();
                return View(model);
            }
            // If we got this far, something failed, redisplay form
            ViewBag.Roles = Roles.GetAllRoles();
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            
            ViewBag.Roles = Roles.GetAllRoles();
            
            NguoiDung nguoidung = db.NguoiDungs.Find(id);

            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            if (Roles.GetRolesForUser(nguoidung.UserName).Contains("KhachHang"))
            {
                return HttpNotFound();
            }

            foreach (string item in Roles.GetRolesForUser(nguoidung.UserName))
            {
                ViewBag.UserRoles += item;
            }
            ViewBag.gioitinh = new SelectList(gioitinh, "Value", "Text", nguoidung.GioiTinh);
            return View(nguoidung);
        }

        [HttpPost]
        public ActionResult Edit(NguoiDung nguoidung, DateTime ngayvaolam)
        {
            ViewBag.gioitinh = new SelectList(gioitinh, "Value", "Text", nguoidung.GioiTinh);
            if (ModelState.IsValid)
            {
                db.Entry(nguoidung).State = EntityState.Modified;
                NhanVien nv = db.NhanViens.Where(n => n.UserId == nguoidung.UserId).Single();
                nv.NgayVaoLam = ngayvaolam;
                db.SaveChanges();

                String[] oldRole = Roles.GetRolesForUser(nguoidung.UserName);
                if (oldRole.Length > 0)
                {
                    Roles.RemoveUserFromRoles(nguoidung.UserName, oldRole);
                }

                foreach (String role in Request.Form.Keys)
                {
                    if (role.StartsWith("chk"))
                    {
                        String rolename = role.Substring(3);
                        Roles.AddUserToRole(nguoidung.UserName, rolename);
                    }
                }

                return RedirectToAction("Index");
            }
            return View(nguoidung);
        }

        public ActionResult Details(int id = 0)
        {
            NguoiDung nguoidung = db.NguoiDungs.Find(id);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = Roles.GetRolesForUser(nguoidung.UserName);
            return View(nguoidung);
        }

        public ActionResult Delete(int id = 0)
        {
            NguoiDung ng = db.NguoiDungs.Find(id);
            ViewBag.Roles = Roles.GetRolesForUser(ng.UserName);
            if (ng == null)
            {
                return HttpNotFound();
            }
            return View(ng);
        }

        
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteNhanVien(int Id)
        {
            var model = db.NguoiDungs.Find(Id);
            String[] oldRole = Roles.GetRolesForUser(model.UserName);
            if (oldRole.Length > 0)
            {
                Roles.RemoveUserFromRoles(model.UserName, oldRole);
            }
            Roles.AddUserToRole(model.UserName, "NghiViec");
            return RedirectToAction("Index");
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
    }
}
