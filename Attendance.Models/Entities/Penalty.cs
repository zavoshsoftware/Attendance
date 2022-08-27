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

        [Display(Name = "مدرک اخذشده")]
        public PenaltyType PenaltyType { get; set; }

        [Display(Name = "کارت")]
        public Guid CardId { get; set; }

        [Display(Name = "رفع توقیف شده/نشده")]
        public bool Solved { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }

    }
    public enum PenaltyType
    {
        [Display(Name = "کارت ملی")]
        NationalCard = 0,
        [Display(Name = "گواهینامه")]
        DrivingLicense = 1,
        [Display(Name = "کارت ورود")]
        LoginCard = 2
    }
}