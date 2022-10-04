using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaoDinhVu.Context;
using CaoDinhVu.Library;

namespace CaoDinhVu.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        private CaoDinhVu067Entities db = new CaoDinhVu067Entities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            ViewBag.listCategory = db.Categorys.ToList();
            ViewBag.listBrand = db.Suppliers.ToList();
            return View(db.Products.Where(p=>p.Status!=0).ToList());
        }
        public String getNameCategory(int id)
        {
            var category = db.Categorys.Where(c => c.Id == id).FirstOrDefault();
            return (category.Name);
        }
        public String getNameBrand(int id)
        {
            var brand = db.Suppliers.Where(c => c.Id == id).FirstOrDefault();
            return (brand.Name);
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Where(p=>p.Slug == slug).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryName = getNameCategory(product.Id);
            ViewBag.brandName = getNameBrand(product.Id);
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.listCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.listSup = new SelectList(db.Suppliers.ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CategoryId,SupplierId,Slug,Image,Detail,Number,Price,PriceSale,Metakey,Metadesc,Created_at,Created_by,Updated_at,Updated_by,Status")] Product product)
        {
            
            if (ModelState.IsValid)
            {
                
                product.Slug = XString.Str_Slug(product.Name);
                var img = Request.Files["img"];
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    if (!FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        ModelState.AddModelError("Erro", "Kiểu tập tin không đúng, Kiểu tập tin hợp lệ là: " + string.Join(",", FileExtentions));
                    }
                    else
                    {
                        //upload hình
                        string imgName = product.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        product.Image = imgName;
                        string PathDir = "~/Content/images/items";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                if (product.Detail == null)
                    product.Detail = product.Name;
                product.Created_by = Session["UserId"].ToString();
                product.Created_at = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.listSup = new SelectList(db.Suppliers.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.listCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.listSup = new SelectList(db.Suppliers.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CategoryId,SupplierId,Slug,Image,Detail,Number,Price,PriceSale,Metakey,Metadesc,Created_at,Created_by,Updated_at,Updated_by,Status")] Product product)
        {

            if (ModelState.IsValid)
            {
                product.Slug = XString.Str_Slug(product.Name);


                //upload file
                var img = Request.Files["img"];

                if (img.FileName.Length != 0)
                {
                    //Session["Test"] = "ddd"+img.FileName;
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    if (!FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        ModelState.AddModelError("Err", "Kiểu tập tin không đúng, Kiểu tập tin hợp lệ là: " + string.Join(",", FileExtentions));
                    }
                    else
                    {
                        //upload hình
                        string imgName = product.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        product.Image = imgName;
                        //Session["Test"] = img.FileName;
                        string PathDir = "~/Content/images/items";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Xóa file
                        if (product.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), product.Image);
                            System.IO.File.Delete(DelPath);
                        }
                        img.SaveAs(PathFile);
                    }
                }
                else
                {
                    product.Image = Session["imgOld"].ToString();
                }
                product.Updated_by = Session["UserId"].ToString();
                product.Updated_at = DateTime.Now;
                TempData["Message"] = new XMessage("success", "sua thành công.");

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "products");
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "products");
            }
            product.Status = 0 /*(product.Status == 1) ? 0 : 1*/;
            product.Updated_by = Session["UserId"].ToString();
            product.Updated_at = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = new XMessage("success", "thêm vao thùng rác thành công.");
            return RedirectToAction("Index", "products");
        }
        //
        public ActionResult ReTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Trash", "products");
            }
            Product product = db.Products.Find(id);
            if(product==null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Trash", "products");
            }
            product.Status = 1;
            product.Updated_by = Session["UserId"].ToString();
            product.Updated_at = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = new XMessage("success", "Khối phục thành công.");
            return RedirectToAction("Trash", "products");
        }
        //
        public ActionResult Trash()
        {
            var list = db.Products.Where(m=>m.Status == 0).ToList();
            return View("Trash", list);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "products");
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "products");
            }
            product.Status = (product.Status == 1) ? 2 : 1;
            product.Updated_by = Session["UserId"].ToString();
            product.Updated_at = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công.");
            return RedirectToAction("Index", "products");
        }
    }
}
