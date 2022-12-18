using Attendance.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

 
    public static class Configurations
    {
        private static T Setting<T>(this string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
        public static decimal MaxLoadAmount
        {
            get
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var _value = db.Configs.FirstOrDefault(c => c.Key == "MaxLoadAmount");
                    if (_value !=null)
                    {
                    return decimal.Parse(_value.Value);
                    }      
                }
                //api base url
                return Setting<decimal>("MaxLoadAmount");
            }
        }
    
        public static string BaseUrl
        {
            get
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var _value = db.Configs.FirstOrDefault(c => c.Key == "BaseUrl");
                    if (_value !=null)
                    {
                    return _value.Value;
                    }      
                }
                //api base url
                return Setting<string>("BaseUrl");
            }
        }
        public static string SecurityCode
    {
            get
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var _value = db.Configs.FirstOrDefault(c => c.Key == "SecurityCode");
                    if (_value !=null)
                    {
                    return _value.Value;
                    }      
                }
                //api base url
                return Setting<string>("SecurityCode");
            }
        }
    } 