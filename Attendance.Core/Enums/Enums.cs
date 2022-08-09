using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Enums
{
   public class Enums
    {
        [Serializable]
        [ComVisible(true)]
        public enum DayOfWeek
        {
            [Display(Name ="شنبه")]
            Saturday = 0,

            [Display(Name ="یکشنبه")]
            Sunday = 1,

            [Display(Name ="دوشنبه")]
            Monday = 2,

            [Display(Name ="سشنبه")]
            Tuesday = 3,

            [Display(Name ="چهارشنبه")]
            Wednesday = 4,

            [Display(Name ="پنج شنبه")]
            Thursday = 5, 

            [Display(Name ="جمعه")]
            Friday = 6
        
        }
    }
}
