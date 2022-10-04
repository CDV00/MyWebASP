using CaoDinhVu.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Models
{
    public class ProfileAddress
    {
        public User user { get; set; }
        public String tinh { get; set; }
        public String huyen { get; set; }
        public String phuong { get; set; }
        public String chiTiet { get; set; }
    }
}