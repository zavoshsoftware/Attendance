using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class CarTypeStatusHistory : BaseEntity
    {
        [Display(Name="وضعیت قبلی")]
        public bool PreviousStatus { get; set; }

        [Display(Name="وضعیت فعلی")]
        public bool CurrentStatus { get; set; }

        [Display(Name="کد کارت")]
        public Guid CarTypeId { get; set; }

        [ForeignKey("CarTypeId")]
        public virtual CarType CarType { get; set; } 

         

    }
}
