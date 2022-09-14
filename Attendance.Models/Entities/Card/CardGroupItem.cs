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
   public class CardGroupItem : BaseEntity
    {

        [Display(Name = "عنوان")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }

        public Guid GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual CardGroup Group { get; set; }
        public virtual ICollection<CardGroupItemCard> GroupItemCards { get; set; }
    }
}
