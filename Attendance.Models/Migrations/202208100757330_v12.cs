namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardLoginHistories", "DriverId", c => c.Guid());
            CreateIndex("dbo.CardLoginHistories", "DriverId");
            AddForeignKey("dbo.CardLoginHistories", "DriverId", "dbo.Drivers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardLoginHistories", "DriverId", "dbo.Drivers");
            DropIndex("dbo.CardLoginHistories", new[] { "DriverId" });
            DropColumn("dbo.CardLoginHistories", "DriverId");
        }
    }
}
