using Attendance.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class CardOwnerHistory : BaseEntity
    {
        [Display(Name = "صاحب قبلی")]
        public Guid PreviousDriver { get; set; }

        [Display(Name = "صاحب بعدی")]
        public Guid CurrentDriver { get; set; }

        [Display(Name = "کد کارت")]
        public Guid CardId { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
    }
}
