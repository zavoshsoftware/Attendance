using System;
using Attendance.Core.Enums;
using Attendance.Models.Entities;

namespace Attendance.Models
{
    internal static class DatabaseContextInitializer
    {
        static DatabaseContextInitializer()
        {

        }

        internal static void Seed(DatabaseContext databaseContext)
        {
            InsertBaseUser(databaseContext);
            databaseContext.SaveChanges();
        }


     
        internal static void InsertBaseUser(DatabaseContext databaseContext)
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                SecurityRole = SecurityRole.Admin,
                CellNum = "09124806404",
                Password = "zavosh123321",
                CreationDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                FullName = "admin",
            };

            databaseContext.Users.Add(user);
        }
    }
}
