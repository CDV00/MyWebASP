using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaoDinhVu.Context;

namespace CaoDinhVu.Areas.Admin.Controllers
{
    public class SlidersController : BaseController
    {
        private CaoDinhVu067Entities _Context = new CaoDinhVu067Entities();

        // GET: Admin/Sliders
        public ActionResult Index()
        {
            return View(_Context.Sliders.ToList());
        }

        // GET: Admin/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = _Context.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Link,Position,Img,Attribute,Sort_order,Orders,Created_at,Created_by,Updated_at,Updated_by,Status")] Slider slider)
        {
            if (ModelState.IsValid)
            {
                _Context.Sliders.Add(slider);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = _Context.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Link,Position,Img,Attribute,Sort_order,Orders,Created_at,Created_by,Updated_at,Updated_by,Status")] Slider slider)
        {
            if (ModelState.IsValid)
            {
                _Context.Entry(slider).State = EntityState.Modified;
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = _Context.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = _Context.Sliders.Find(id);
            _Context.Sliders.Remove(slider);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
