using Attendance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Attendance.Web.Services.Errors
{
    public class ErrorService : IErrorService
    {
        public string HandleError(Exception exception, string info)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                string errors = exception.Message + exception.InnerException;
                db.ExceptionLoggers.Add(new Models.Entities.ExceptionLoggers()
                {
                    Description = errors,
                    ExceptionMessage = exception.Message,
                    ExceptionStackTrace = exception.StackTrace,
                    RouteName = info,
                    Time = DateTime.Now
                });
                db.SaveChanges();
            return errors;
            }
        }

        public string HandleError(DbEntityValidationException exception,string info)
        {
            var errors = string.Join("\n", exception.EntityValidationErrors.SelectMany(e => e.ValidationErrors).Select(ve => $"PropertyName:{ve.PropertyName} \t ErrorMessage:{ve.ErrorMessage}"));
            using (DatabaseContext db=new DatabaseContext())
            {
                db.ExceptionLoggers.Add(new Models.Entities.ExceptionLoggers()
                {
                    Description=errors,
                    ExceptionMessage=exception.Message,
                    ExceptionStackTrace=exception.StackTrace,
                    RouteName= info,
                    Time=DateTime.Now
                });
                db.SaveChanges();
            }
            return  errors;
        }
    }
}