using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
   public class Card:BaseEntity
    {
        [Display(Name="کد کارت")]
        public string Code { get; set; }
        [Display(Name="راننده")]
        public Guid DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public virtual ICollection<CardDay> CardDays { get; set; }
        public virtual ICollection<CardLoginHistory> CardLoginHistories { get; set; }
    }
}
