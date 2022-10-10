using Attendance.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Attendance.Web.ViewModels
{
    public class LoginHistoryDetailsViewModel
    {
        [Display(Name = "تاریخ ورود")]
        public DateTime LoginDate { get; set; }

        [Display(Name = "موفق؟")]
        public bool IsSuccess { get; set; }

        [Display(Name = "کد کارت")]
        public Guid? CardId { get; set; }

        public Guid Id { get; set; }
         

        [Display(Name = "تاریخ خروج")]
        public DateTime? ExitDate { get; set; }

        [Display(Name = "نام راننده")]
        public string DriverName { get; set; }

        public string Description { get; set; }


        [Display(Name = "شماره کارت")]
        public string CarNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "بار")]
        public decimal Load { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "مقدار واقعی")]
        public decimal? TotalLoad { get; set; }

        [Display(Name = "نام کمک راننده")]
        public string AssistanceName { get; set; }

        [Display(Name = "نام خانوادگی کمک راننده")]
        public string AssistanceLastName { get; set; }

        [Display(Name = "کدملی کمک راننده")]
        public string AssistanceNationalCode { get; set; }

        [Display(Name = "خودرو")]
        public Guid? CarId { get; set; }


        [Display(Name = "وسایل همراه ")]
        [DataType(DataType.MultilineText)]
        public string Devices { get; set; }
          
        [Display(Name = "راننده")]
        public Guid? DriverId { get; set; }
         

        [Display(Name = "ورود")]
        public Guid CardLoginHistoryId { get; set; }

        [Display(Name = "وسایل")]
        public Guid ToolId { get; set; }

        [Display(Name = "واحد")]
        public Guid UnitId { get; set; }

        [Display(Name = "مقدار")]
        public double Amount { get; set; }
         
        public Driver Driver { get; set; }

         
        public virtual Car Car { get; set; }

        public virtual Card Card { get; set; }
    }
}