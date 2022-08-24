using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string NationalCode { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<CardLoginHistory> CardLoginHistories { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
