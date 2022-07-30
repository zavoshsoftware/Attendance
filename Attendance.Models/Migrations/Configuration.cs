
namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Attendance.Models.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Attendance.Models.DatabaseContext databaseContext)
        {
            base.Seed(databaseContext);
            if (databaseContext.Users.Count() != 0)
            {
                return;
            }

            try
            {
                DatabaseContextInitializer.Seed(databaseContext);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
