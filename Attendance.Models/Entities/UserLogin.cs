using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class UserLogin : BaseEntity
    {
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public bool IsSuccessLogin { get; set; }
    }
}
