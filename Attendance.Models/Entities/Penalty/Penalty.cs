using Attendance.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class Penalty : BaseEntity
    {
        [Display(Name = "علت")]
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        [Display(Name = "مدرک توقیف شده")]
        public PenaltyType PenaltyType { get; set; }
         
        [Display(Name = "کارت")]
        public Guid CardId { get; set; }

        [Display(Name = "علت")]
        public Guid? ReasonId { get; set; }

        [Display(Name = "رفع توقیف شده/نشده")]
        public bool Solved { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

        [ForeignKey("ReasonId")]
        public virtual PenaltyReason PenaltyReason { get; set; }

    }

}