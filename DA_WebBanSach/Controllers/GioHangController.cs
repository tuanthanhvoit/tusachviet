using DA_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_WebBanSach.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/

        SachDbContext db = new SachDbContext();

        public ActionResult Index()
        {
            var model = GioHang.Cart;
            return View(model);
        }

        public ActionResult AddtoCart(int id = 0, int sl=1)
        {
            GioHang.Cart.ThemGio(id, sl);
            String response = "fnUpdateCartInfo('"+GioHang.Cart.Count+"', '"+GioHang.Cart.Amount.ToString("0,0")+"')";
                
                //String.Format("fnUpdateCartInfo({0}, {1})",
                //GioHang.Cart.Count, );
            return Content(response);
        }

        public ActionResult Remove(int id = 0)
        {
            GioHang.Cart.Remove(id);
            String response = String.Format("fnUpdateCartInfo('{0}', '{1}')",
                GioHang.Cart.Count, GioHang.Cart.Amount.ToString("0,0;-0.0;0"));
            return Content(response);
        }


        public ActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Manage(String btnAction = null)
        {
            if (btnAction == "Cập Nhật")
            {
                String[] names = Request.Form.AllKeys;
                foreach (var name in names)
                {
                    if (name.StartsWith("qty"))
                    {
                            int id = int.Parse(name.Substring(3));
                            if (Request.Form[name].Length < 3)
                            {
                                try
                                {
                                    int quantity = int.Parse(Request.Form[name]);
                                    if (quantity < 0)
                                    {
                                        ViewBag.Error = "Số Lượng Không Đươc < 0";
                                    }
                                    else {
                                        Sach s = db.Saches.Find(id);
                                        if (s.SoLuongTon > quantity)
                                        {
                                            GioHang.Cart.Update(id, quantity);
                                        }
                                        else
                                        {
                                            ViewBag.Error = "Số Lượng Sách Không Đủ Vui Lòng Kiểm Tra Lại";
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    ViewBag.Error = "Số Lượng Không Đươc Nhập Chữ";
                                }
                                
                            }
                            else {
                                ViewBag.Error = "Số Lượng Nhập Quá Lớn";
                            }                     
                    }
                }
            }
            else if (btnAction == "Xóa Tất Cả")
            {
                GioHang.Cart.Clear();
            }
            else if (btnAction == "Tiếp Tục Mua Sách")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (btnAction == "Thanh Toán")
            {
                return RedirectToAction("Index", "ThanhToan");
            }
            return View();
        }
    }
}
