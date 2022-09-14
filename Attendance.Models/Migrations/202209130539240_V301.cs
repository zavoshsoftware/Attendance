namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V301 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "BirthDate");
        }
    }
}
