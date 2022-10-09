using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class CarStatusHistory : BaseEntity
    {
        [Display(Name="وضعیت قبلی")]
        public bool PreviousStatus { get; set; }

        [Display(Name="وضعیت فعلی")]
        public bool CurrentStatus { get; set; }
         
        public Guid CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; } 

         

    }
}
