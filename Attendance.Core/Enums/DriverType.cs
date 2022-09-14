using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Enums
{
    public enum DriverType
    {
        [Display(Name = "راننده")]
        Driver = 0,

            [Display(Name = "کمک راننده")]
        Assistance = 1

    }
}
