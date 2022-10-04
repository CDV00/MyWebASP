using CaoDinhVu.Context;
using CaoDinhVu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CaoDinhVu.Controllers
{
    public class CartController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["cart"];
            
            var list = new List<CartModel>();
            if (cart != null)
            {
                list = (List<CartModel>)cart;
                for (int i = 0; i < list.Count;i++)
                {
                    var product = _context.Products.Find(list[i].Product.Id);
                    list[i].nameBrand = _context.Suppliers.Where(s => s.Id == product.SupplierId).Select(s => s.Name).FirstOrDefault();
                    list[i].nameCategory = _context.Categorys.Where(c => c.Id == product.CategoryId).Select(c => c.Name).FirstOrDefault();
                }
            }
                return View(list);
        }
        public ActionResult AddToCart(int id, int? quantity = 1)
        {
            var product = _context.Products.Find(id) ;
            
            if (Session["Cart"] != null)
            {
                List<CartModel> cart = (List<CartModel>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity += quantity.Value;
                    //gán vào sesstion
                    Session["cart"] = cart;
                    return Json(new { Message = "ThemSoLuong", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    //tạo mới đối tượng cart item
                    //List<CartModel> Cart = new List<CartModel>();
                    cart.Add(new CartModel { Product = product, Quantity = quantity.Value });
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                    //gán vào sesstion
                    Session["cart"] = cart;
                    return Json(new { Message = "ThemSanPham", JsonRequestBehavior.AllowGet });
                }
                

            }
            else
            {
                //tạo mới đối tượng cart item
                List<CartModel> Cart = new List<CartModel>();

                Cart.Add(new CartModel { Product = product, Quantity = quantity.Value });
                //gán vào sesstion
                Session["cart"] = Cart;
                Session["count"] = 1;
                return Json(new { Message = "TaoMoi", JsonRequestBehavior.AllowGet });
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet});
        }
        private int isExist(int Id)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(Id))
                    return i;
            }
            return -1;
        }
        //update
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartModel>>(cartModel);
            var sesstionCart = (List<CartModel>)Session["cart"];

            foreach (var item in sesstionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session["cart"] = sesstionCart;
            return Json(new
            {
                status = true,
            });
        }
        
        public JsonResult Delete(long id)
        {
            var sesstionCart = (List<CartModel>)Session["cart"];
            sesstionCart.RemoveAll(x => x.Product.Id == id);
            Session["cart"] = sesstionCart;
            Session["count"] = Int32.Parse(Session["count"].ToString())-1;
            return Json(new
            {
                status = true,
            });
            
        }
        public JsonResult DeleteAll()
        {
            Session["cart"] = null;
            @Session["count"] = 0;
            return Json(new
            {
                status = true,
            });
        }
        public ActionResult _InfoDHTC()
        {
            Order orderDAO = new Order();
            //int customerID = int.Parse(Session["customerId"].ToString());
            var list = _context.Orders.OrderByDescending(o => o.Id).Where(m => m.Status == 1).FirstOrDefault();
            return View("_InfoDHTC", list);
        }
    }
    
}