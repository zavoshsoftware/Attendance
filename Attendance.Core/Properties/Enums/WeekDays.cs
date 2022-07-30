using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Enums
{
    public enum WeekDays
    {
        [Display(Name = "شنبه")] Saturday = 0,
        [Display(Name = "یک شنبه")] Sunday = 1,
        [Display(Name = "دوشنبه")] Monday = 2,
        [Display(Name = "سه شنبه")] Tuesday = 3,
        [Display(Name = "چهار شنبه")] Wednesday = 4,
        [Display(Name = "پنج شنبه")] Thursday = 5,
        [Display(Name = "جمعه")] Friday = 6,
    }
}
