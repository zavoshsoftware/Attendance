using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class PenaltyReason : BaseEntity
    {
        [Display(Name = "علت")] 
        public string Title { get; set; }

        public virtual ICollection<Penalty> Penalties { get; set; }
    }
    
}