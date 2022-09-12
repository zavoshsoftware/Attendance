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
   public class CardGroupItemCard : BaseEntity
    { 
        public Guid CardGroupItemId { get; set; }
        public Guid CardId { get; set; }

        [ForeignKey("CardGroupItemId")]
        public virtual CardGroupItem GroupItem { get; set; } 

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; } 
    }
}
