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
           case 'e':
                result = string.Format("{0}/{1}/{2}-{3}:{4}:{5}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d), pc.GetHour(d).ToString("00"), pc.GetMinute(d).ToString("00"), pc.GetSecond(d).ToString("00"));
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

    public static string DayName(this DateTime date)
    {
        Dictionary<string, string[]> DayOfWeeks = new Dictionary<string, string[]>();
        DayOfWeeks.Add("en", new string[] { "Saturday", "Sunday", "Monday", "Tuesday", "Thursday", "Wednesday", "Friday" });
        DayOfWeeks.Add("fa", new string[] { "شنبه", "یک شنبه", "دو شنبه", "سه شنبه", "چهار شنبه", "پنج شنبه", "جمعه" });
        return DayOfWeeks["fa"][DayOfWeekReformat((int)date.DayOfWeek)];
    }

    public static DateTime ToMiladi(this string shamsi)
    {
        PersianCalendar persianCalendar = new PersianCalendar();
        int[] date = shamsi.Split('/').Select(s=>int.Parse(s)).ToArray();
        DateTime dt = new DateTime(date[0], date[1], date[2], persianCalendar);
        return dt;
    }

    public static int DayOfWeekReformat(this int day)
    { 
        if (day == 0) return 1;
        else if (day == 1) return 2;
        else if (day == 2) return 3;
        else if (day == 3) return 4;
        else if (day == 4) return 5;
        else if (day == 5) return 6;
        else return 0;
    }

    public static int GetAge(this DateTime? birthdate)
    {
        if (birthdate.HasValue)
        {
            var days = (DateTime.Now - birthdate)?.TotalDays;
            return Convert.ToInt32(days / 365);
        }
        return 0;
    }
} 
