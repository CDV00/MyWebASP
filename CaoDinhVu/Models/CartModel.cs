using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string nameBrand { get; set; } ="";
        public string nameCategory { get; set; } = "";
    }
}