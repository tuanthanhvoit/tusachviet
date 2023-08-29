using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class Report
    {
        public int IName { get; set; }
        public String SName { get; set; }
        public String Name
        {
            get
            {
                return IName > 0 ? IName.ToString() : SName;
            }
        }
        public int TongSL { get; set; }
        public double TongTien { get; set; }
    }

    public class Report2
    {
        public int name { get; set; }
        public int TongSL { get; set; }
        public double TongTien { get; set; }
    }

    public class TongKet
    {
        public int TongSL { get; set; }
        public double TongTien { get; set; }
    }

    public class TongKetThang
    {
        int stt { get; set; }
        public int Thang { get; set; }
        public int TongSL { get; set; }
        public double TongTien { get; set; }
    }

    public class SoLuongSachBan
    {
        public string TenSach { get; set; }
        public int Count { get; set; }
    }

    public class NhapThongKe
    {
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int Year { get; set; }
    }
}