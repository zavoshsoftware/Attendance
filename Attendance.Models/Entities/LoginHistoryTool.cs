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
    public class LoginHistoryTool : BaseEntity
    {
        [Display(Name = "ورود")] 
        public Guid CardLoginHistoryId { get; set; }
         
        [Display(Name = "وسایل")] 
        public Guid? ToolId { get; set; }
         
        [Display(Name = "واحد")] 
        public Guid? UnitId { get; set; }

        [Display(Name = "مقدار")]
        public double? Amount { get; set; }

        [ForeignKey("CardLoginHistoryId")]
        public CardLoginHistory CardLoginHistory { get; set; }

        [ForeignKey("ToolId")]
        public Tools Tools { get; set; }

        [ForeignKey("UnitId")]
        public Units Units { get; set; }
    }
}
