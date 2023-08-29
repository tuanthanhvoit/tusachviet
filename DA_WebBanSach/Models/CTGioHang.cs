using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class CTGioHang
    {
        public int SachID { get; set; }
        public string TenSach { get; set; }
        public string Hinh { get; set; }
        [Range(0,99,ErrorMessage="Số lượng nhập từ 0 đến 99")]
        public int SoLuong { get; set; }
        public float Gia { get; set; }
    }
}