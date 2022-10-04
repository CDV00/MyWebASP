using CaoDinhVu.Context;
using CaoDinhVu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    public class CategoryController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Category
        public ActionResult Index()
        {
            var listCategory = categoryList().ToList();
            
            //var a = listCategory.ToList();
           //var listCategory = _context.Categorys.ToList();
            return View(listCategory);
        }
        public IQueryable<Category> categoryList()
        {
            var listCategory = _context.Categorys.AsQueryable();
            return listCategory;
        }
        public IQueryable<Category> categoryListId()
        {
            var listCategory = _context.Categorys.Where(a=>a.Id <= 2);
            return listCategory;
        }
        public ActionResult ProductCategory(String slug, int? page = 1)
        {
            int pageSize = 4;
            
            var Category = _context.Categorys.Where(c=>c.Slug == slug).FirstOrDefault();
            int totalProduct = _context.Products.Where(p => p.CategoryId == Category.Id && p.Status == 1).AsQueryable().Count();
            int totalPage = totalProduct / pageSize;
            if (totalProduct % pageSize > 0)
                totalPage += 1;
            var listProduct = _context.Products.Where(p => p.CategoryId == Category.Id && p.Status == 1).OrderBy(p=>p.Id).Skip((page.Value - 1)*4).Take(4).ToList();
            ResponsesCategory category = new ResponsesCategory();
            ViewBag.categoryName = Category.Name;
            ViewBag.slugCategory = slug;
            ViewBag.page = page;
            ViewBag.pageNext = page+1;
            ViewBag.pagePrevious = page-1;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;
            ViewBag.totalProduct = totalProduct;
            return View(listProduct);
        }
    }
}