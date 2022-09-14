namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V31 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Drivers", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Drivers", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
