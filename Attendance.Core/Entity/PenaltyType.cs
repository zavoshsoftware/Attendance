using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Entity
{
    public enum PenaltyType
    {
        [Display(Name = "کارت ملی")]
        NationalCard = 0,
        [Display(Name = "گواهینامه")]
        DrivingLicense = 1,
        [Display(Name = "کارت ورود")]
        LoginCard = 2
    }
     
}
