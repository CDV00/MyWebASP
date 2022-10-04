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
    public class BrandController : BaseController
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();

        // GET: Admin/Brand
        public ActionResult Index()
        {
            return View(_context.Suppliers.Where(s=>s.Status != 0).ToList());
        }

        // GET: Admin/Brand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Admin/Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,Image,ParentId,Orders,Title,MetaKey,MetaDesc,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {

                supplier.Slug = XString.Str_Slug(supplier.Name);
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
                        string imgName = supplier.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        supplier.Image = imgName;
                        string PathDir = "~/Content/images/brands";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                if (supplier.MetaDesc == null)
                    supplier.MetaDesc = supplier.Name;
                supplier.CreatedBy = "Admin";
                supplier.CreatedAt = DateTime.Now;
                supplier.MetaKey = supplier.MetaDesc;

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: Admin/Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,Image,ParentId,Orders,Title,MetaKey,MetaDesc,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Status")] Supplier supplier)
        {
            
            if (ModelState.IsValid)
            {
                supplier.Slug = XString.Str_Slug(supplier.Name);
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
                        string imgName = supplier.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        supplier.Image = imgName;
                        //Session["Test"] = img.FileName;
                        string PathDir = "~/Content/images/brand";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Xóa file
                        if (supplier.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Image);
                            System.IO.File.Delete(DelPath);
                        }
                        img.SaveAs(PathFile);
                    }
                }
                else
                {
                    supplier.Image = Session["imgOld"].ToString();
                }
                supplier.MetaKey = supplier.MetaDesc;
                supplier.UpdatedBy = "Admin";
                supplier.UpdatedAt = DateTime.Now;
                



                _context.Entry(supplier).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["Message"] = new XMessage("success", "sua thành công.");
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Admin/Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = _context.Suppliers.Find(id);
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            //Xóa hinh
            if (supplier.Image != null)
            {
                string PathDir = "~/Content/images/brands";
                string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Image);
                System.IO.File.Delete(DelPath);
            }
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
                return RedirectToAction("Index", "Brand");
            }
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Brand");
            }
            supplier.Status = (supplier.Status == 1) ? 2 : 1;
            supplier.UpdatedBy = "Admin";
            supplier.UpdatedAt = DateTime.Now;
            _context.Entry(supplier).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công.");
            return RedirectToAction("Index", "Brand");
        }
        //
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Brand");
            }
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index", "Brand");
            }
            supplier.Status = 0 /*(product.Status == 1) ? 0 : 1*/;
            supplier.UpdatedBy = "Admin";
            supplier.UpdatedAt = DateTime.Now;
            _context.Entry(supplier).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["message"] = new XMessage("success", "thêm vao thùng rác thành công.");
            return RedirectToAction("Index", "Brand");
        }
        //
        public ActionResult ReTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Trash", "Brand");
            }
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Trash", "Brand");
            }
            supplier.Status = 1;
            supplier.UpdatedBy = "Admin";
            supplier.UpdatedAt = DateTime.Now;
            _context.Entry(supplier).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["message"] = new XMessage("success", "Khối phục thành công.");
            return RedirectToAction("Trash", "Brand");
        }
    }
}
