using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
   public static class EngToPers
    {
        public static string ConvertDigitEngToPers(this string d)
=> d.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵")
    .Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹").Replace("0", "۰");


    } 
