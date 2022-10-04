using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    public class ProductController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Product
        public ActionResult ProductDetail(int id)
        {
            var product = _context.Products.Where(p => p.Id == id).SingleOrDefault();
            return View(product);
        }
        public ActionResult GetAll(int? page=1)
        {
            int pageSize = 8;
            int totalProduct = _context.Products.Where(p => p.Status == 1).AsQueryable().Count();
            int totalPage = totalProduct / pageSize;
            if (totalProduct % pageSize > 0)
                totalPage += 1;
            var listProduct = _context.Products.Where(p => p.Status == 1).OrderBy(p => p.Id).Skip((page.Value - 1) * pageSize).Take(pageSize).ToList();
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