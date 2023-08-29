using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace DA_WebBanSach.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AreaAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
                //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                //{
                //    var area = filterContext.RouteData.DataTokens["area"];
                //    filterContext.HttpContext.Response.Redirect("/" + area.ToString() + "/Login/Index?ReturnUrl="
                //                        + HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                //}
                base.OnAuthorization(filterContext);
                if (filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary 
                        { 
                                { "Areas", filterContext.RouteData.Values[ "area" ] }, 
                                { "controller", "Login" }, 
                                { "action", "Index" }, 
                                { "ReturnUrl", filterContext.HttpContext.Request.RawUrl } 
                        });
                }
        }
    }
}