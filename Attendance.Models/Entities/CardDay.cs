using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Core.Enums;
using Helpers;

namespace Attendance.Models.Entities
{
   public class CardDay:BaseEntity
    {
        [Display(Name = "روز هفته")]
        public WeekDays WeekDays { get; set; }
        public Guid CardId { get; set; }
        public virtual Card Card { get; set; }

        [NotMapped]
        [Display(Name = "روز هفته")]
        public string CardDaysStr
        {
            get
            {
                return EnumExtensions.GetDisplayName(WeekDays);
            }
        }
    }
}
