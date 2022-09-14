using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
  public  class ExceptionLoggers
    {
        [Key]
        public long Id { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string RouteName { get; set; } 
        public string Description { get; set; }
        public DateTime Time { get; set; }
    }
}
