namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "BirthDate");
        }
    }
}
