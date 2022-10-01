using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class Penalty_PenaltyType : BaseEntity
    {
        public Guid PenaltyTypeId { get; set; }
        public Guid PenaltyId { get; set; }


        [ForeignKey("PenaltyTypeId")]
        public virtual PenaltyType PenaltyType { get; set; }

        [ForeignKey("PenaltyId")]
        public virtual Penalty Penalty { get; set; }
    }
    
}