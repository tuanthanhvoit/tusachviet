using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class SachKM
    {
        public Sach Sach { get; set; }
        public double GiaGiam { get; set; }
        public double GiaBan { get; set; }

        SachDbContext db = new SachDbContext();

        public List<SachKM> capnhatKM(List<SachKM> query)
        {
            foreach (var item in query)
            {
                var khuyenmai = db.ChiTietKhuyenMais
                    .Where(c => c.SachID == item.Sach.SachID && c.KhuyenMai.NgayBatDau < DateTime.Now && c.KhuyenMai.NgayKetThuc > DateTime.Now)
                    .SingleOrDefault();
                double giaban = double.Parse(item.Sach.DonGia.ToString());
                if (khuyenmai != null)
                {
                    item.GiaGiam = khuyenmai.KhuyenMai.GiaGiam;
                    item.GiaBan = (1 - item.GiaGiam) * giaban;
                }
                else
                {
                    item.GiaGiam = 1;
                    item.GiaBan = item.GiaGiam * giaban;
                }
            }
            return query;
        }

        public SachKM capnhatKM(SachKM query)
        {
                var khuyenmai = db.ChiTietKhuyenMais
                    .Where(c => c.SachID == query.Sach.SachID && c.KhuyenMai.NgayBatDau < DateTime.Now && c.KhuyenMai.NgayKetThuc > DateTime.Now)
                    .SingleOrDefault();
                double giaban = double.Parse(query.Sach.DonGia.ToString());
                if (khuyenmai != null)
                {
                    query.GiaGiam = khuyenmai.KhuyenMai.GiaGiam;
                    query.GiaBan = (1 - query.GiaGiam) * giaban;
                }
                else
                {
                    query.GiaGiam = 1;
                    query.GiaBan = query.GiaGiam * giaban;
                }
            
            return query;
        }
    }
}