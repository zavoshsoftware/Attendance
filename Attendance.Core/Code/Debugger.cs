using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Code
{ 
    public static class Debugger
    {
        public static String Info(
      [CallerFilePath] string file = null,
      [CallerLineNumber] int lineNumber = 0,
      [CallerMemberName] string method = null)
        {
            String location = file;
            if (lineNumber != 0)
            {
                location += "(" + lineNumber.ToString() + ")";
            }
            if (!String.IsNullOrWhiteSpace(method))
            {
                location += ": " + method;
            }
            if (!String.IsNullOrWhiteSpace(location))
            {
                location += ": ";
            }
            return location;
        }
    }
}
