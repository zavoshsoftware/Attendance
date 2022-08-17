using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Enums
{
    
        public enum WalkingLoginHistoryType
    {
            [Display(Name ="ورود")]
            Enter = 1,

            [Display(Name ="خروج")]
            Exit = 2

             
        
        }
  
}
