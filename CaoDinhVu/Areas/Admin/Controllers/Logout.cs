using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Areas.Admin.Controllers
{
    public class Logout
    {
        public Logout()
        {
            System.Web.HttpContext.Current.Response.Redirect("~/Auth/logout");
        }
    }
}