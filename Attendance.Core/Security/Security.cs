using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Security
{
   public class Security
    {

        public static string GenerateCardDisplayCode(DayOfWeek day, string nationalCode)
            => $"{day}-{nationalCode.Substring(nationalCode.Length - 7)}";
    }
}
