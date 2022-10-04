using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Models
{
    public class OrderModel
    {
        public List<Order> listOrders { get; set; }
        public List<Orderdetail> listOrderdetails { get; set; }
        public List<Product> listProduct { get; set; }
        public List<Supplier> listBrand { get; set; }
        public List<Category> listCategory { get; set; }
    }
}