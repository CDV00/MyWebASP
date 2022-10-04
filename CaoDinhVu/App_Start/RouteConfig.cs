using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CaoDinhVu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "dang xuat",
               url: "dang-xuat",
               defaults: new { controller = "Auth", action = "Logout" },
                new[] { "CaoDinhVu.Controllers" }
           );
            routes.MapRoute(
               name: "dang nhap",
               url: "dang-nhap",
               defaults: new { controller = "Auth", action = "Login"},
                new[] { "CaoDinhVu.Controllers" }
           );
            routes.MapRoute(
               name: "thanh-toan",
               url: "thanh-toan",
               defaults: new { controller = "Payment", action = "Index", page = 1 },
                new[] { "CaoDinhVu.Controllers" }
           );
            routes.MapRoute(
               name: "tim-kiem",
               url: "tim-kiem/{category}/{key}/{page}",
               defaults: new { controller = "Search", action = "FilterProduct", page = 1 },
                new[] { "CaoDinhVu.Controllers" }
           );
            routes.MapRoute(
                name: "gio-hang",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", page = 1 },
                new[] { "CaoDinhVu.Controllers" }
            );
            routes.MapRoute(
                name: "tat-ca-san-pham",
                url: "tat-ca-san-pham/{page}",
                defaults: new { controller = "Product", action = "GetAll" ,page =1},
                new[] { "CaoDinhVu.Controllers" }
            );
            routes.MapRoute(
                name: "thuong hieu",
                url: "thuong-hieu/{slug}/{page}",
                defaults: new { controller = "Brand", action = "ProductBrand", page = 1 },
                new[] { "CaoDinhVu.Controllers" }
            );
            routes.MapRoute(
                name: "tat ca thuong hieu",
                url: "thuong-hieu",
                defaults: new { controller = "Brand", action = "Index", page = 1 },
                new[] { "CaoDinhVu.Controllers" }
            );
            routes.MapRoute(
                name: "danh muc",
                url: "danh-muc/{slug}/{page}",
                defaults: new { controller = "Category", action = "ProductCategory", page = 1 },
                new[] { "CaoDinhVu.Controllers" }
            );
            routes.MapRoute(
                name: "chi tiet san pham",
                url: "chi-tiet-san-pham/{id}",
                defaults: new { controller = "Product", action = "ProductDetail", id = UrlParameter.Optional },
                new[] { "CaoDinhVu.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "CaoDinhVu.Controllers"}
            );
        }
    }
}
