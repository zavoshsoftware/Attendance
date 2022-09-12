using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class Config:BaseEntity
    {
        [Display(Name = "شناسه")]
        public int Code { get; set; }

        [Display(Name = "کلید")] 
        public string Key { get; set; }

        [Display(Name = "مقدار")]
        public string Value { get; set; }
    }
}
