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
    public class Units : BaseEntity
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")] 
        public string Title { get; set; }

        [Display(Name = "الویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")] 
        public int Order { get; set; }

        public int MyProperty { get; set; }

        public virtual ICollection<LoginHistoryTool> LoginHistoryTools { get; set; }
    }
}
