using Attendance.Models;
using Attendance.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                //penalty 
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

                //
                if (db.Configs.Any())
                {
                    db.Configs.AddRange(new List<Config>() 
                    { new Config() { Key = "MaxLoadAmount", Value = Configurations.MaxLoadAmount.ToString() } ,
                     new Config() { Key = "BaseUrl", Value = Configurations.BaseUrl} ,
                    });
                }
            }

            //Run card reader 
            string domain = Configurations.BaseUrl;
            var path = System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            Process p = new Process();
            p.StartInfo.FileName = path.Replace("bin", $"ReaderAttrib\\GetReaderAttrib.exe");
            p.StartInfo.Arguments = domain;
            //p.Start();
        }
    }
}
