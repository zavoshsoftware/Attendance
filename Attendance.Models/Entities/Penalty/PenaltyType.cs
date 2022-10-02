using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class PenaltyType : BaseEntity
    {
        [Display(Name = "عنوان")] 
        public string Title { get; set; }

        public virtual ICollection<Penalty_PenaltyType> Penalty_PenaltyTypes { get; set; }
    }
    
}