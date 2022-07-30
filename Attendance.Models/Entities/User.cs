

using Helpers;

namespace Attendance.Models.Entities
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class User : BaseEntity
    {
        public User()
        {
            UserLogins = new List<UserLogin>();
        }
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(20, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string CellNum { get; set; }

        [Display(Name = "پسورد")]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "نقش")]
        public string SecurityRole { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}