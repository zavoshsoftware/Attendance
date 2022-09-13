using Attendance.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
   public class Driver:BaseEntity
    {
        [Display(Name="نام")]
        public string FirstName { get; set; }

        [Display(Name="نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name="شماره موبایل")]
        public string CellNumber { get; set; }


        [Display(Name = "نام پدر")]
        public string Father { get; set; }

        [Display(Name="کدملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(10,ErrorMessage ="مقدار {0} باید {1} رقم باشد")]
        [MaxLength(10,ErrorMessage ="مقدار {0} باید {1} رقم باشد")]
        public string NationalCode { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; } 
        [Display(Name = "نوع")]
        public DriverType DriverType { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<CardLoginHistory> CardLoginHistories { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
