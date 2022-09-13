using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
  public  class SystemLog
    {
        [Key]
        public long Id { get; set; }
        public string Log { get; set; }


        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } 
        public DateTime Time { get; set; }
    }
}
