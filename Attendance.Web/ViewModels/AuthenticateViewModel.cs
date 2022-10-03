using Attendance.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public class Select2Model
{
    public string id { get; set; }
    public string text { get; set; }
}
public class AuthenticateViewModel
    {

    } 
    public class AuthenticateFormViewModel
    {

    public string CarType { get; set; }
    public Guid LoginId { get; set; } 
    public Guid cardId { get; set; }
    public Guid? carId { get; set; }
    public Guid AssistanceId { get; set; }

    public Card Card { get; set; }

    public Driver Driver { get; set; }

    [Display(Name = "نام")]
  [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
      public string DriverFirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
 [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
      public string DriverLastName { get; set; }
    
    [Display(Name = "کدملی")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public string DriverNatCode { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    [Display(Name = "بار")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public decimal Load { get; set; }

    public Car Car { get; set; }

    [Display(Name = "شماره پلاک")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public string Pleck { get; set; }
    
    [Display(Name = "خودرو")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public string Type { get; set; }
    
    [Display(Name = "خطا")]
    public bool Err { get; set; }

    public string ErrMessage { get; set; }

    [Display(Name = "برند")]
    public string Brand { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    [Display(Name = "وزن")]
    public decimal Weight { get; set; }


    [Display(Name = "نام")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public string AssistanceName { get; set; }
    
    [Display(Name = "نام خانوادگی")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public string AssistanceLastName { get; set; }
    
    [Display(Name = "کدملی")]
     [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
    public string AssistanceNationalCode { get; set; }

} 

