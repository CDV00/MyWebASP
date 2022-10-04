using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Models
{
    public class AccountModel
    {
        public User user { get; set; }
        public List<Order> listOrder { get; set; }
        public int donHang { get; set; }
        public int choXacNhan { get; set; }
        public int danggiao { get; set; }
        public int daGiao { get; set; }
    }
}