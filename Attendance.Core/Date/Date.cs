using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
   public static class Date
    {
    public static string ToShamsi(this DateTime d, char type = 'a')
    {
        PersianCalendar pc = new PersianCalendar();
        string result = string.Empty;
        switch (type)
        {
            case 'a':
                result = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
                break;
            case 'h':
                result = string.Format("{0}:{1}", pc.GetHour(d.AddHours(4)), pc.GetMinute(d.AddMinutes(30)));
                break;
           case 's':
                result = string.Format("{0}/{1}/{2}-{3}:{4}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d), pc.GetHour(d), pc.GetMinute(d));
                break;

        }
        return result;
    }
} 
