using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
   public class Car:BaseEntity
    {
        [Display(Name = "نوع ماشین")]
        public Guid CarTypeId{ get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} باشد")]
        public string Title  { get; set; }

        [Display(Name = "پلاک")] 
        public string  Number { get; set; }

        [ForeignKey("CarTypeId")]
        public CarType CarType { get; set; }

        public ICollection<CardLoginHistory> CardLoginHistories { get; set; }
    }
}
