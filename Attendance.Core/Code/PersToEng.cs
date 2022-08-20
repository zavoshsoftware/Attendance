using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
   public static class PersToEng
    { 
    public static string ConvertDigit(this string d)
    => d.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("٤", "4").Replace("۵", "5").Replace("٥", "5")
        .Replace("۶", "6").Replace("٦", "6").Replace("۷", "6").Replace("۸", "8").Replace("۹", "9").Replace("۰", "0");
         
         
} 
