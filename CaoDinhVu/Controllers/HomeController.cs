using CaoDinhVu.Context;
using CaoDinhVu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    public class HomeController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Home
        public ActionResult Index()
        {
            //var originValue = Request.Headers["Origin"].FirstOrDefault();

            //// or

            ///*StringValues originValues;
            //Request.Headers.TryGetValue("Origin", out originValues);*/
            //Console.WriteLine(originValue);

            HomeModel homeModel = new HomeModel();
            homeModel.ListProducts = _context.Products.Where(p=>p.Status == 1).ToList();
            homeModel.ListCategories = _context.Categorys.Where(c => c.Status == 1).ToList();
            homeModel.ListSliders = _context.Sliders.Where(s => s.Status == 1).OrderBy(s=>s.Orders).ThenBy(s=>s.Id).ToList();
            homeModel.ListSupplier = _context.Suppliers.Where(s => s.Status == 1).ToList();
            
            return View(homeModel);
        }
        public ActionResult _Menu()
        {
            HomeModel homeModel = new HomeModel();
            homeModel.ListCategories = _context.Categorys.Where(c => c.Status == 1).ToList();
            homeModel.ListSupplier = _context.Suppliers.Where(s => s.Status == 1).ToList();
            return View(homeModel);
        }
    }
}