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
                result = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d).ToString("00"), pc.GetDayOfMonth(d).ToString("00"));
                break;
            case 'h':
                result = string.Format("{0}:{1}", pc.GetHour(d.AddHours(4)).ToString("00"), pc.GetMinute(d).ToString("00"));
                break;
           case 's':
                result = string.Format("{0}/{1}/{2}-{3}:{4}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d), pc.GetHour(d).ToString("00"), pc.GetMinute(d).ToString("00"));
                break;

        }
        return result;
    }
    public static string ToShamsi(this DateTime? date, char type = 'a')
    {
        if (!date.HasValue)
        {
            return "";
        }
        return ToShamsi(date.Value,type);
    }
} 
