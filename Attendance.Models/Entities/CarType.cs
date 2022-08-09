using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Models.Entities
{
   public class CarType:BaseEntity
    {
        public CarType()
        {

        }
        [Display(Name ="عنوان")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }
        
        [Display(Name ="برند")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} باشد")]
        public string Brand { get; set; }

        [Display(Name = "وزن")]
        public decimal Weight { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
