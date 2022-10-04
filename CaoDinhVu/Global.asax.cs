using CaoDinhVu.Context;
using CaoDinhVu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CaoDinhVu
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            Session["UserAdmin"] ="";
            Session["UserId"] = "";
            Session["Email"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            Session["customerName"] = "";
            Session["customerId"] = "";
            Session["cart"] = new List<CartModel>();
            Session["count"] = (int)0;
            Session["cat"] = 1;
            Session["imgOld"] = "";
            Session["TongGia"] = (decimal) 0;
            Session["Location"] = "";
        }
    }
}
