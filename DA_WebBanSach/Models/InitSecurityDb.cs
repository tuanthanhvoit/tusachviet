using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using DA_WebBanSach.Models;
using WebMatrix.WebData;

namespace DA_WebBanSach.Models
{
    public class InitSecurityDb : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {

            WebSecurity.InitializeDatabaseConnection("SachDBTest",
               "NguoiDungs", "UserId", "UserName", autoCreateTables: true);
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if (!roles.RoleExists("NhanVien"))
            {
                roles.CreateRole("NhanVien");
            }
            if (!roles.RoleExists("BanHang"))
            {
                roles.CreateRole("BanHang");
            }
            if (!roles.RoleExists("HoaDon"))
            {
                roles.CreateRole("HoaDon");
            }
            if (!roles.RoleExists("NghiViec"))
            {
                roles.CreateRole("NghiViec");
            }
            if (!roles.RoleExists("KhachHang"))
            {
                roles.CreateRole("KhachHang");
            }
            if (membership.GetUser("Admin", false) == null)
            {
                WebSecurity.CreateUserAndAccount("Admin", "123456", propertyValues: new { 
                    Email = "tuanthanhvo.1989@gmail.com",
                    HoTen = "Võ Tuấn Thành",
                    GioiTinh = true,
                    NgaySinh = DateTime.Parse("1989/09/08"),
                    DienThoai = "01664084112",
                    DiaChi = "024 e cư xá thanh đa",
                    Quan = "Bình Thạnh",
                    ThanhPho = "Hồ Chí Minh"
                });
            }
            if (!roles.GetRolesForUser("Admin").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "Admin" }, new[] { "Admin" });
            }
        }
    }
}