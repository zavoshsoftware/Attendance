using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
         
        [Display(Name= "روز هفته")]
        [DisplayName("روز هفته")]
        public Attendance.Core.Enums.WeekDays Day { get; set; }


        [Display(Name = "کد نمایشی")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نباید بیشتر از {1} باشد")]
        public string DisplayCode { get; set; }

        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }
        public virtual ICollection<CardLoginHistory> CardLoginHistories { get; set; }
    }
}
