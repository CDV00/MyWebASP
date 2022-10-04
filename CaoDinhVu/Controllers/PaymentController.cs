using CaoDinhVu.Context;
using CaoDinhVu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    
    public class PaymentController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Payment
        public ActionResult Index()
        {
            if (!Session["FullName"].Equals(""))
            {
                var user = _context.Users.Find(Int32.Parse(Session["UserId"].ToString()));
                Order order = new Order();
                order.DeliveryName = user.FullName;
                order.DeliveryEmail = user.Email;
                order.DeliveryPhone = user.Phone;
                order.DeliveryAddress = user.Address;
                order.UserId = Int32.Parse(Session["UserId"].ToString());
                order.CreatedDate = DateTime.Now;
                order.Status = 2;
                try
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    
                    var id = order.Id; ;
                    var Cart = (List<CartModel>)Session["cart"];
                    var detailDao = new Orderdetail();
                    foreach (var item in Cart)
                    {
                        var orderDetail = new Orderdetail();
                        orderDetail.OrderId = (int)id;
                        orderDetail.ProductId = item.Product.Id;
                        orderDetail.Quantity = item.Quantity;

                        orderDetail.Price = item.Product.Price;
                        orderDetail.Amount = item.Product.Price * item.Quantity;

                        _context.Orderdetails.Add(orderDetail);
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    //ghi log
                    return Redirect("/loi-thanh-toan");
                }
                    return View();
            }
            Session["Location"] = "Payment";
            return RedirectToAction("Login","Auth");
        }
    }
}