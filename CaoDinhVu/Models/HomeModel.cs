using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Models
{
    public class HomeModel
    {
        public List<Category> ListCategories { get; set; }
        public List<Product> ListProducts { get; set; }
        public List<Slider> ListSliders { get; set; }
        public List<Supplier> ListSupplier { get; set; }
    }
}