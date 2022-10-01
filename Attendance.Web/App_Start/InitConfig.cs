using Attendance.Models;
using Attendance.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Attendance.Web
{
    public class InitConfig
    { 
        public static void RegisterData()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                if (!db.PenaltyTypes.Any())
                {
                    db.PenaltyTypes.AddRange(new List<PenaltyType>() { 
                        new PenaltyType()
                        {
                            Id = Guid.NewGuid(),
                            CreationDate=DateTime.Now,
                            IsActive = true,
                            Title = "کارت ملی",
                            IsDeleted=false
                        },
                        new PenaltyType()
                        {
                            Id = Guid.NewGuid(),
                            CreationDate=DateTime.Now,
                            IsActive = true,
                            Title = "گواهینامه",
                            IsDeleted=false
                        }, 
                        new PenaltyType()
                        {
                            Id = Guid.NewGuid(),
                            CreationDate=DateTime.Now,
                            IsActive = true,
                            Title = "کارت ورود",
                            IsDeleted=false
                        }
                    });
                    db.SaveChanges();
                }
            }
        }
    }
}
