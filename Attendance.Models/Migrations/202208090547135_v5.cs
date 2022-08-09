namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Drivers", "Driver_Id", "dbo.Drivers");
            DropIndex("dbo.Drivers", new[] { "Driver_Id" });
            AddColumn("dbo.Cards", "DisplayCode", c => c.String(maxLength: 20));
            AddColumn("dbo.Drivers", "NationalCode", c => c.String());
            DropColumn("dbo.Drivers", "Driver_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drivers", "Driver_Id", c => c.Guid());
            DropColumn("dbo.Drivers", "NationalCode");
            DropColumn("dbo.Cards", "DisplayCode");
            CreateIndex("dbo.Drivers", "Driver_Id");
            AddForeignKey("dbo.Drivers", "Driver_Id", "dbo.Drivers", "Id");
        }
    }
}
