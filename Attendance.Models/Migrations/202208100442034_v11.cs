namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CardLoginHistories", "DriverHelperName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CardLoginHistories", "DriverHelperName", c => c.String());
        }
    }
}
