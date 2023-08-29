using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class PhanTrang
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }


        public int PageCount
        {

            get
            {
                return (int)Math.Ceiling(1.0 * RowCount / PageSize);
            }
        }

        public int LastPageNo
        {
            get
            {
                if (PageCount == 0)
                {
                    return 0;
                }
                else {
                    return PageCount - 1;
                }
            }
        }

        public void Navigate(int PageNo)
        {
            if (PageNo < 0)
            {
                this.PageNo = LastPageNo;
            }
            else if (PageNo > this.LastPageNo)
            {
                this.PageNo = 0;
            }
            else
            {
                this.PageNo = PageNo;
            }
        }

        public static PhanTrang Get(String id, int PageSize = 10)
        {
            PhanTrang pi = HttpContext.Current.Session[id] as PhanTrang;

            if (pi == null) // chua ton tai trong session
            {
                pi = new PhanTrang
                {
                    PageSize = PageSize
                };

                HttpContext.Current.Session[id] = pi;
            }

            return pi;


        }
    }
}