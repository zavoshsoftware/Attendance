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

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public DbSet<User> Users { get; set; }
     
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Card> Cards { get; set; } 
        public DbSet<CardLoginHistory> CardLoginHistories { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<WalkingLoginHistory> WalkingLoginHistories  { get; set; }
        public DbSet<Config> Configs { get; set; }

        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<ExceptionLoggers> ExceptionLoggers { get; set; }
        public DbSet<CardStatusHistory> CardStatusHistories { get; set; }
        public DbSet<CardGroup> Groups { get; set; }
        public DbSet<CardGroupItem> GroupItems{ get; set; } 
        public DbSet<CardGroupItemCard> CardGroupItemCards { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<PenaltyReason> PenaltyReason { get; set; }
        public DbSet<DriverStatusHistory>  DriverStatusHistories { get; set; }
        public DbSet<CarTypeStatusHistory>  CarTypeStatusHistories { get; set; }
        public DbSet<CarStatusHistory> CarStatusHistories { get; set; }
        public DbSet<CardOwnerHistory> CardOwnerHistories { get; set; }
        public DbSet<PenaltyType> PenaltyTypes { get; set; }
        public DbSet<Penalty_PenaltyType> Penalty_PenaltyTypes { get; set; }
        public DbSet<Tools> Tools { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<LoginHistoryTool> LoginHistoryTools { get; set; }
         
    }
}
