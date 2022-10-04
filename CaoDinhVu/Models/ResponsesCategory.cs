using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaoDinhVu.Models
{
    public class ResponsesCategory
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public int totalProduct{ get; set; }
    }
}