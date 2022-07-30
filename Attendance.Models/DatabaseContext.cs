using System.Data.Entity;
using Attendance.Models.Entities;

namespace Attendance.Models
{
    public class DatabaseContext : DbContext
    {
        static DatabaseContext()
        {
       System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<User> Users { get; set; }
     
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardDay> CardDays { get; set; }
        public DbSet<CardLoginHistory> CardLoginHistories { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
