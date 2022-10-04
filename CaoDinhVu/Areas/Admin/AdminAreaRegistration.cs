using System.Web.Mvc;

namespace CaoDinhVu.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "chitietsanpham",
                "Admin/chi-tiet-san-pham/{slug}",
                new { controller = "Products", action = "Details", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "CaoDinhVu.Areas.Admin.Controllers" }
            );
        }
    }
}