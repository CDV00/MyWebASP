using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CaoDinhVu.Library
{
    public class XString
    {
        public static string Str_Slug(string s)
        {
            string[][] symbols ={
                new string[]{"[áàảãạăắằẳẵặâấầẩẫậ]","a"},
                new string[]{"[đ]","d"},
                new string[]{"[éèẻẽẹêếềểễệ]","e"},
                new string[]{"[íìỉĩị]","i"},
                new string[]{"[óòỏõọôốồổỗộơớờởỡợ]","o"},
                new string[]{"[úùủũụứừửữự]","u"},
                new string[]{"[ýỳỷỹỵ]","y"},
                new string[]{"[\\s'\";,]","-"}
            };
            s = s.ToLower();
            foreach (var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }
    }
}