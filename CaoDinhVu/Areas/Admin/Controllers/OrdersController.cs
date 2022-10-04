using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaoDinhVu.Context;
using CaoDinhVu.Library;

namespace CaoDinhVu.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private CaoDinhVu067Entities db = new CaoDinhVu067Entities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,UserId,CreatedDate,ExportDate,DeliveryAddress,DeliveryName,DeliveryPhone,DeliveryEmail,Updated_at,Updated_by,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại.");
                return RedirectToAction("Index");
            }
            order.Status = (order.Status == 1) ? 2 : 1;
            order.Updated_by = Session["UserId"].ToString();
            order.Updated_at = DateTime.Now;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công.");
            return RedirectToAction("Index");
        }
    }
}
