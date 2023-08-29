using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            SachDbContext db = new SachDbContext();
            var email = db.NguoiDungs.SingleOrDefault(u => u.Email.ToLower() == ((string)value).ToLower());
            return email == null;
        }

    }
}