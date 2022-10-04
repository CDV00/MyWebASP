using CaoDinhVu.Context;
using CaoDinhVu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    public class AccountController : Controller
    {
        private CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Account
        public AccountController()
        {
            if (System.Web.HttpContext.Current.Session["UserId"].Equals(""))
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Auth/login");
            }
        }
        [AllowAnonymous]
        public ActionResult ProfileAddress()
        {
            new AccountController();
            ProfileAddress profileAddress = new ProfileAddress();
            int userId = Int32.Parse(Session["UserId"].ToString());
            profileAddress.user = _context.Users.Find(userId);
            string address = profileAddress.user.Address;

            /*int index;
            int length = address.Length;
            index =  address.LastIndexOf(",");*/
            int index = address.LastIndexOf(",");
            string s = address.Substring(index + 1);
            address.Remove(10);
            profileAddress.tinh = XuLyChuoi(ref address);
            profileAddress.huyen = XuLyChuoi(ref address);
            profileAddress.phuong = XuLyChuoi(ref address);
            profileAddress.chiTiet = address;

            /*index = address.LastIndexOf(",");
            profileAddress.tinh = address.Substring(index);*/
            return View(profileAddress);
        }
        private String XuLyChuoi(ref string address)
        {
            int index = address.LastIndexOf(",");
            string s = address.Substring(index + 2);
            address = address.Remove(index);
            return s;
        }
        // GET: /Account/Profile-Main
        [AllowAnonymous]
        public ActionResult ProfileMain()
        {
            new AccountController();
            int userId = Int32.Parse(Session["UserId"].ToString());
            AccountModel accountModel = new AccountModel();
            accountModel.user = _context.Users.Find(userId);
            accountModel.donHang = _context.Orders.Where(o => o.UserId == userId).Count();
            accountModel.choXacNhan = _context.Orders.Where(o => o.UserId == userId && o.Status == 2).Count();
            accountModel.danggiao = _context.Orders.Where(o => o.UserId == userId && o.Status == 1).Count();
            accountModel.daGiao = _context.Orders.Where(o => o.UserId == userId && o.Status == 3).Count();
            accountModel.listOrder = _context.Orders.Where(o => o.UserId == userId && o.Status != 0).ToList();
            return View(accountModel);
        }
        // GET: /Account/Profile-Order
        [AllowAnonymous]
        public ActionResult ProfileOrder()
        {
            new AccountController();
            OrderModel orderModel = new OrderModel();
            int userId = Int32.Parse(Session["UserId"].ToString());
            
            orderModel.listOrders = _context.Orders.Where(o => o.UserId == userId && o.Status != 0).ToList();

            List<Orderdetail> lstOrderdetail = new List<Orderdetail>();
            List<Product> lstProduct = new List<Product>();
            List<Supplier> lstBrand = new List<Supplier>();
            List<Category> lstCategory = new List<Category>();
            //order detail
            for (int i = 0; i < orderModel.listOrders.Count(); i++)
            {
                int orderId = orderModel.listOrders[i].Id;
                lstOrderdetail.AddRange(_context.Orderdetails.Where(o => o.OrderId == orderId).ToList());
            }
            orderModel.listOrderdetails = lstOrderdetail;

            //product 
            for (int i = 0; i < orderModel.listOrderdetails.Count(); i++)
            {
                int productId = orderModel.listOrderdetails[i].ProductId;
                if(checkProductExist(lstProduct, productId))
                {
                    lstProduct.Add(_context.Products.Where(o => o.Id == productId).FirstOrDefault());
                }
                 
            }
            orderModel.listProduct = lstProduct;
            //Brand
            for (int i = 0; i < orderModel.listProduct.Count(); i++)
            {
                int brandId = orderModel.listProduct[i].SupplierId;
                if (checkBrandExist(lstBrand, brandId))
                {
                    lstBrand.Add(_context.Suppliers.Where(o => o.Id == brandId).FirstOrDefault());
                }

            }
            orderModel.listBrand = lstBrand;
            //Category
            for (int i = 0; i < orderModel.listProduct.Count(); i++)
            {
                int categoryId = orderModel.listProduct[i].CategoryId;
                if (checkCategoryExist(lstCategory, categoryId))
                {
                    lstCategory.Add(_context.Categorys.Where(o => o.Id == categoryId).FirstOrDefault());
                }

            }
            orderModel.listCategory = lstCategory;
            //Category
            /*var orderDetail = _context.Orderdetails.ToList();
             int i = 0;
            foreach (var item in orderDetail)
            {
                if(i< orderModel.listOrders.Count())
                {
                    if(item.OrderId == orderModel.listOrders[i].Id)
                        lst.Add(item);
                }
                i++;
            }*/


            return View(orderModel);
        }
        private bool checkProductExist(List<Product> list, int id)
        {
            for (int p = 0; p < list.Count(); p++)
            {
                if (list[p].Id == id)
                {
                    return false;
                }
            }
                
                    return true;
        }

        private bool checkBrandExist(List<Supplier> list, int id)
        {
            for (int p = 0; p < list.Count(); p++)
            {
                if (list[p].Id == id)
                {
                    return false;
                }
            }

            return true;
        }
        //
        private bool checkCategoryExist(List<Category> list, int id)
        {
            for (int p = 0; p < list.Count(); p++)
            {
                if (list[p].Id == id)
                {
                    return false;
                }
            }

            return true;
        }
        // GET: /Account/Profile-Seller
        [AllowAnonymous]
        public ActionResult ProfileSeller()
        {
            return View();
        }
        // GET: /Account/Profile-Setting
        [AllowAnonymous]
        public ActionResult ProfileSetting()
        {
            new AccountController();
            ProfileAddress profileAddress = new ProfileAddress();
            int userId = Int32.Parse(Session["UserId"].ToString());
            profileAddress.user = _context.Users.Find(userId);
            string address = profileAddress.user.Address;

            /*int index;
            int length = address.Length;
            index =  address.LastIndexOf(",");*/
            
            profileAddress.tinh = XuLyChuoi(ref address);
            profileAddress.huyen = XuLyChuoi(ref address);
            profileAddress.phuong = XuLyChuoi(ref address);
            profileAddress.chiTiet = address;

            /*index = address.LastIndexOf(",");
            profileAddress.tinh = address.Substring(index);*/
            return View(profileAddress);
        }
        public ActionResult DoSetting(FormCollection field){
            new AccountController();
            int userId = Int32.Parse(Session["UserId"].ToString());
            var user = _context.Users.Find(userId);

            String fullName = field["fullName"];
            String userName = field["userName"];
            String email = field["email"];
            String tinh = field["Tinh"];
            String huyen = field["Huyen"];
            String phuong = field["phuong"];
            String soNha = field["soNha"];

            user.FullName = fullName;
            user.UserName = userName;
            user.Email = email;
            user.Address = soNha + ", " + phuong + ", " + huyen + ", " + tinh;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("ProfileSetting");
        }
    }
}