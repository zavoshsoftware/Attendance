using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models.Entities
{
    public class CardLoginHistory : BaseEntity
    {
        public DateTime LoginDate { get; set; }
        public bool IsSuccess { get; set; }
        public Guid? CardId { get; set; }
        public virtual Card Card { get; set; }
        public DateTime? ExitDate { get; set; }
        public string DriverName { get; set; }
        public string DriverHelperName { get; set; }
        public string CarNumber { get; set; }
        public decimal? TotalLoad { get; set; }
    }
}
