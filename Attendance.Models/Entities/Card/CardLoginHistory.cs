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
        [Display(Name="تاریخ ورود")]
        public DateTime LoginDate { get; set; }

        [Display(Name="موفق؟")]
        public bool IsSuccess { get; set; }

        [Display(Name="کد کارت")]
        public Guid? CardId { get; set; }

        public virtual Card Card { get; set; }

        [Display(Name = "تاریخ خروج")]
        public DateTime? ExitDate { get; set; }

        [Display(Name="نام راننده")]
        public string DriverName { get; set; }
         

        [Display(Name="شماره کارت")]
        public string CarNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name="بار")]
        public decimal Load { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "مجموع بار")]
        public decimal? TotalLoad { get; set; }

        [Display(Name = "نام کمک راننده")]
        public string AssistanceName { get; set; }

        [Display(Name = "نام خانوادگی کمک راننده")]
        public string AssistanceLastName { get; set; }

        [Display(Name = "کدملی کمک راننده")]
        public string AssistanceNationalCode { get; set; }

        [Display(Name = "خودرو")]
        public Guid? CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }



        [Display(Name = "راننده")]
        public Guid? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }


    }
}
