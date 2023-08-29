using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class GioHangHD
    {
        SachDbContext db = new SachDbContext();
        public List<ChiTietHoaDon> _Items { get; set; }

        public static GioHangHD CartAdmin
        {
            get
            {
                var cart = HttpContext.Current.Session["CartHD"];
                if (cart == null)//neu gio hang rong thi tao moi
                {
                    cart = new GioHangHD
                    {
                        _Items = new List<ChiTietHoaDon>()
                    };
                    //luu vao session
                    HttpContext.Current.Session["CartHD"] = cart;
                }
                return (GioHangHD)cart;
            }
        }

        public void ThemGio(ChiTietHoaDon ct)
        {
            _Items.Add(ct);
        }

        public void Clear()
        {
            _Items.Clear();
        }

        public void Remove(int id)
        {
            ChiTietHoaDon ct = _Items.Single(p => p.ChiTietHoaDonID == id);
            _Items.Remove(ct);
        }

        public void Update(int id, int soluong)
        {
            ChiTietHoaDon prod = _Items.Single(p => p.ChiTietHoaDonID == id);
            Sach s = db.Saches.Find(prod.SachID);
            if (s.SoLuongTon >= soluong)
            {
                prod.SoLuong = soluong;
            }
        }

        public int Count
        {
            get
            {
                if (_Items == null)
                {
                    return 0;
                }
                return _Items.Count;
            }
        }

        public double Amount
        {
            get
            {
                if (_Items == null)
                {
                    return 0;
                }
                return (double)_Items.Sum(p => p.SoLuong * p.DonGia);
            }
        }
    }
}