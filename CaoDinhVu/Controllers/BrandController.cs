using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    public class BrandController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Brand
        public ActionResult Index()
        {
            var lisBrand = _context.Suppliers.ToList();
            return View(lisBrand);
        }
        public ActionResult ProductBrand(String slug, int? page = 1)
        {
            int pageSize = 4;
            var Brand = _context.Suppliers.Where(c => c.Slug == slug).FirstOrDefault();
            int totalProduct = _context.Products.Where(p => p.SupplierId == Brand.Id && p.Status == 1).AsQueryable().Count();
            int totalPage = totalProduct / pageSize;
            if (totalProduct % pageSize > 0)
                totalPage += 1;
            var listProduct = _context.Products.Where(p => p.SupplierId== Brand.Id && p.Status == 1).OrderBy(p => p.Id).Skip((page.Value - 1) * 4).Take(4).ToList();
            ViewBag.BrandName = Brand.Name;
            ViewBag.slugBrand = slug;
            ViewBag.page = page;
            ViewBag.pageNext = page + 1;
            ViewBag.pagePrevious = page - 1;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;
            ViewBag.totalProduct = totalProduct;
            return View(listProduct);
        }
    }
}