using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Web.Services.Errors
{
   public interface IErrorService
    {
        string HandleError(Exception exception, string info);
        string HandleError(DbEntityValidationException exception, string info);
    }
}
