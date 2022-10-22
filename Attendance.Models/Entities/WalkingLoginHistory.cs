using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Core.Enums;

namespace Attendance.Models.Entities
{
    public class WalkingLoginHistory:BaseEntity
    {
        [Display(Name="نوع")]
        public WalkingLoginHistoryType WalkingLoginHistoryType { get; set; }
        public Guid CardId { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }

        [Display(Name="راننده")]
        public bool IsDriver { get; set; }
        [Display(Name="کمک راننده")]
        public bool IsDriverHelper { get; set; }

        [NotMapped]
        [Display(Name="نوع")]
        public string WalkingLoginHistoryTypeStr
        {
            get
            {
                return EnumExtensions.GetDisplayName(WalkingLoginHistoryType);
            }
        }

    }
}
