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
    public Guid cardId { get; set; }
    public Card Card { get; set; }
    public Driver Driver { get; set; }
    [Display(Name = "نام و نام خانوادگی")]
      public string DriverFullName { get; set; }
    
    [Display(Name = "کدملی")]
    public string DriverNatCode { get; set; }

    public Car Car { get; set; }

    [Display(Name = "شماره پلاک")]
    public string Pleck { get; set; }
    
    [Display(Name = "نوع")]
    public string Type { get; set; }

    
    [Display(Name = "برند")]
    public string Brand { get; set; }

    
    [Display(Name = "وزن")]
    public decimal Weight { get; set; }


    [Display(Name = "نام")]
    public string AssistanceName { get; set; }
    
    [Display(Name = "نام خانوادگی")]
    public string AssistanceLastName { get; set; }
    
    [Display(Name = "کدملی")]
    public string AssistanceNationalCode { get; set; }

} 

