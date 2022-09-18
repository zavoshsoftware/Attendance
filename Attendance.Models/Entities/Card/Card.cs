using Attendance.Core.Enums;
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
    public class Card : BaseEntity
    {
        [Display(Name = "کد سیستمی کارت")]
        public string Code { get; set; }

        [Display(Name = "صاحب کارت")]
        public Guid DriverId { get; set; }


        [Display(Name = "روز هفته")]
        [DisplayName("روز هفته")]
        public Attendance.Core.Enums.WeekDays Day { get; set; }

        public string DayStr
        {
            get
            {
              return  Day.GetDisplayName();
            }
        }
        [Display(Name = "کارت مخفی است؟")]
        public bool IsHidden { get; set; }

        [Display(Name = "کد نمایشی")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نباید بیشتر از {1} باشد")]
        public string DisplayCode { get; set; }

        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }
        public virtual ICollection<CardLoginHistory> CardLoginHistories { get; set; }
        public virtual ICollection<CardStatusHistory> CardStatusHistories { get; set; }
        public virtual ICollection<CardGroupItemCard> CardGroupItems { get; set; }
        public virtual ICollection<Penalty> Penalties { get; set; }
    }
}
