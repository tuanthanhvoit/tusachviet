using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class GioHangAdmin
    {
        public List<ChiTietDonHang> _Items { get; set; }
        SachDbContext db = new SachDbContext();
        public static GioHangAdmin CartAdmin
        {
            get
            {
                var cart = HttpContext.Current.Session["CartAdmin"];
                if (cart == null)//neu gio hang rong thi tao moi
                {
                    cart = new GioHangAdmin
                    {
                        _Items = new List<ChiTietDonHang>()
                    };
                    //luu vao session
                    HttpContext.Current.Session["CartAdmin"] = cart;
                }
                return (GioHangAdmin)cart;
            }
        }

        public void ThemGio(ChiTietDonHang ct)
        {
            _Items.Add(ct);
        }

        public void Clear()
        {
            _Items.Clear();
        }

        public void Remove(int id)
        {
            ChiTietDonHang ct = _Items.Single(p => p.ChiTietDonHangID == id);
            _Items.Remove(ct);
        }

        public void Update(int id, int soluong)
        {
            ChiTietDonHang prod = _Items.Single(p => p.ChiTietDonHangID == id);
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