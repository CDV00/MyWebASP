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
    public class CategoriesController : BaseController
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(_context.Categorys.Where(c=>c.Status != 0).ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,ParentId,Orders,Image,Title,MetaKey,MetaDesc,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Status")] Category category)
        {
            if (ModelState.IsValid)
            {

                category.Slug = XString.Str_Slug(category.Name);
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
                        string imgName = category.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        category.Image = imgName;
                        string PathDir = "~/Content/images/items";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                if (category.MetaDesc == null)
                    category.MetaDesc = category.Name;
                category.CreatedBy = "Admin";
                category.CreatedAt = DateTime.Now;
                category.MetaKey = category.MetaDesc;
                _context.Categorys.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,ParentId,Orders,Image,Title,MetaKey,MetaDesc,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Status")] Category category)
        {
            
            if (ModelState.IsValid)
            {
                category.Slug = XString.Str_Slug(category.Name);
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
                        string imgName = category.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        category.Image = imgName;
                        //Session["Test"] = img.FileName;
                        string PathDir = "~/Content/images/items";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Xóa file
                        if (category.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), category.Image);
                            System.IO.File.Delete(DelPath);
                        }
                        img.SaveAs(PathFile);
                    }
                }
                else
                {
                    category.Image = Session["imgOld"].ToString();
                }
                category.UpdatedBy = "Admin";
                category.CreatedAt = DateTime.Now;
                
                category.MetaKey = category.MetaDesc;


                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["Message"] = new XMessage("success", "sua thành công.");
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            //Xóa hinh
            if (category.Image != null)
            {
                string PathDir = "~/Content/images/items";
                string DelPath = Path.Combine(Server.MapPath(PathDir), category.Image);
                System.IO.File.Delete(DelPath);
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _context.Categorys.Find(id);
            _context.Categorys.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Trash()
        {
            var list = _context.Categorys.Where(m => m.Status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Categories");
            }
            var category = _context.Categorys.Find(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Categories");
            }
            category.Status = (category.Status == 1) ? 2 : 1;
            category.UpdatedBy = "Admin";
            category.UpdatedAt = DateTime.Now;
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công.");
            return RedirectToAction("Index", "Categories");
        }
        //
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Categories");
            }
            var category = _context.Categorys.Find(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Categories");
            }
            category.Status = 0 /*(product.Status == 1) ? 0 : 1*/;
            category.UpdatedBy = "Admin";
            category.UpdatedAt = DateTime.Now;
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["message"] = new XMessage("success", "thêm vao thùng rác thành công.");
            return RedirectToAction("Index", "Categories");
        }
        //
        public ActionResult ReTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Trash", "Categories");
            }
            var category = _context.Categorys.Find(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Trash", "Categories");
            }
            category.Status = 1;
            category.UpdatedBy = "Admin";
            category.UpdatedAt = DateTime.Now;
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["message"] = new XMessage("success", "Khối phục thành công.");
            return RedirectToAction("Trash", "Categories");
        }
    }
}
