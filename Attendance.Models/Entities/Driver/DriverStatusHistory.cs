using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class DriverStatusHistory : BaseEntity
    {
        [Display(Name="وضعیت قبلی")]
        public bool PreviousStatus { get; set; }

        [Display(Name="وضعیت بعدی")]
        public bool CurrentStatus { get; set; }

        [Display(Name="کد کارت")]
        public Guid DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; } 

         

    }
}
