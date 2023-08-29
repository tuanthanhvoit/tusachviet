using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class GioHang
    {
        SachDbContext db = new SachDbContext();
        public List<CTGioHang> _Items { get; set; }

        public static GioHang CartDHAdmin
        {
            get
            {
                var cart = HttpContext.Current.Session["CartDHAdmin"];
                if (cart == null)//neu gio hang rong thi tao moi
                {
                    cart = new GioHang
                    {
                        _Items = new List<CTGioHang>()
                    };
                    //luu vao session
                    HttpContext.Current.Session["CartDHAdmin"] = cart;
                }
                return (GioHang)cart;
            }
        }

        public static GioHang Cart
        {
            get
            {
                var cart = HttpContext.Current.Session["GioHang"];
                if (cart == null)//neu gio hang rong thi tao moi
                {
                    cart = new GioHang
                    {
                        _Items = new List<CTGioHang>()
                    };
                    //luu vao session
                    HttpContext.Current.Session["GioHang"] = cart;
                }
                return (GioHang)cart;
            }
        }

        public void ThemGio(int id, int soluong = 1)
        {
            try
            {
                    CTGioHang ct = _Items.Single(p => p.SachID == id);
                    Sach s = db.Saches.Find(ct.SachID);
                    if (s.SoLuongTon >= ct.SoLuong)
                    {
                        ct.SoLuong += soluong;
                    }
            }
            catch
            {
                SachKM s = db.Saches.Where(c => c.SachID == id).Select(c => new SachKM { Sach=c}).SingleOrDefault();
                s = s.capnhatKM(s);
                if (s.Sach.SoLuongTon >= soluong)
                {
                    CTGioHang ct = new CTGioHang
                    {
                        SachID = s.Sach.SachID,
                        TenSach = s.Sach.TenSach,
                        Hinh = s.Sach.AnhBia,
                        SoLuong = soluong,
                        Gia = (float)s.GiaBan
                    };
                    _Items.Add(ct);
                }
            }
        }

        public void Clear()
        {
            _Items.Clear();
        }

        public void Remove(int id)
        {
            CTGioHang ct = _Items.Single(p => p.SachID == id);
            _Items.Remove(ct);
        }

        public void Update(int id, int soluong)
        {
            CTGioHang prod = _Items.Single(p => p.SachID == id);
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
                return _Items.Sum(p => p.SoLuong * p.Gia);
            }
        }

    }
}