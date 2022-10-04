using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace CaoDinhVu.Controllers
{
    public class SearchController : Controller
    {
        CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Search
        public ActionResult FilterProduct(string category = "" , string key ="", int? page=1)
        {
            List<Product> listProduct = null;
            if (category.Equals(""))
                category = "All type";
            int pageSize = 8;
            int totalProduct = 0;
            if (category == "Only best")
            {
                totalProduct = _context.Products.Where(p => p.Name == key).Count();
                listProduct = _context.Products.Where(p => p.Name== key).OrderBy(p=>p.Id).Skip((page.Value-1)*pageSize).Take(pageSize).ToList();

            }
            else
            {
                totalProduct = _context.Products.Where(p => p.Name.Contains(key)).Count();
                listProduct = _context.Products.Where(p => p.Name.Contains(key)).OrderBy(p => p.Id).Skip((page.Value - 1) * pageSize).Take(pageSize).ToList();
            }
            int totalPage = ((totalProduct % pageSize)>0)? (totalProduct / pageSize)+1: totalProduct / pageSize;
            ViewBag.key = key;
            ViewBag.category = category;
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