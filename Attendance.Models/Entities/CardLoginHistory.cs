using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class CardLoginHistory : BaseEntity
    {
        [Display(Name="تاریخ ثبت کارت")]
        public DateTime LoginDate { get; set; }
        [Display(Name="موفق؟")]
        public bool IsSuccess { get; set; }
        [Display(Name="کد کارت")]
        public Guid? CardId { get; set; }
        public virtual Card Card { get; set; }

        public DateTime? ExitDate { get; set; }
        [Display(Name="نام راننده")]
        public string DriverName { get; set; }
        public string DriverHelperName { get; set; }
        [Display(Name="شماره کارت")]
        public string CarNumber { get; set; }
         
        public decimal? TotalLoad { get; set; }

        [Display(Name = "نام")]
        public string AssistanceName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string AssistanceLastName { get; set; }

        [Display(Name = "کدملی")]
        public string AssistanceNationalCode { get; set; }

        [Display(Name = "خودرو")]
        public Guid? CarId { get; set; }
        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }
    }
}
