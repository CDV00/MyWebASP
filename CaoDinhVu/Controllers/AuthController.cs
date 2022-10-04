using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaoDinhVu.Controllers
{
    public class AuthController : Controller
    {
        CaoDinhVu067Entities _context = new CaoDinhVu067Entities();
        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult DoLogin(FormCollection field)
        {
            String email = field["userName"];
            String password = field["password"];
            if (_context.Users.Where(u => u.Email == email).FirstOrDefault() == null)
            {
                ViewBag.Error = "<strong class=\"text-danger \">Email không tồn tại.</strong>";
                return RedirectToAction("Login");
            }
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user.Password != password)
            {
                ViewBag.Error = "<strong class=\"text-danger \">Password sai.</strong>";
                return RedirectToAction("Login");
            }
            Session["customerName"] = user.FullName;
            Session["UserId"] = user.Id;
            Session["Email"] = user.Email;
            Session["FullName"] = user.FullName;


            if (Session["Location"].Equals("Payment"))
            {
                string location = Session["Location"].ToString();
                Session["Location"] = "";
                return RedirectToAction("Index", "Payment");
            }
            if (Session["Location"].Equals("Admin") || user.Roles.Equals("Admin"))
            {
                Session["UserAdmin"] = user.UserName;
                string location = Session["Location"].ToString();
                Session["Location"] = "";
                return Redirect("~/Admin/Dashboard/index");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult DoRegister(FormCollection field)
        {

            String fullName = field["fullName"];
            String userName = field["userName"];
            String email = field["email"];
            String tinh = field["Tinh"];
            String huyen = field["Huyen"];
            String phuong = field["phuong"];
            String soNha = field["soNha"];
            String gender = field["gender"];
            String password = field["password"];
            String repeatPssword = field["repeatPssword"];
            if (_context.Users.Where(u => u.Email == email).FirstOrDefault() != null)
            {

                ViewBag.Error = "<strong class=\"text-danger \">Email đã tồn tại.</strong>";
                return RedirectToAction("Register");
            }
            if (password == repeatPssword)
            {
                User user = new User();
                user.Email = email;
                user.FullName = fullName;
                user.UserName = userName;
                user.Roles = "Custommer";
                user.Address = soNha + ", " + phuong + ", " + huyen + ", " + tinh;
                _context.Users.Add(user);
                _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            ViewBag.Error = "<strong class=\"text-danger \">Mật khẩu xác thực không hợp lệ.</strong>";
            return RedirectToAction("Register");
        }
        public ActionResult Register()
        {

            return View();
        }
        public ActionResult Logout()
        {
            Session["customerName"] ="";
            Session["UserId"] = "";
            Session["Email"] = "";
            Session["FullName"] = "";
            Session["UserAdmin"] = "";
            return RedirectToAction("Index", "Home");
        }
    }
}