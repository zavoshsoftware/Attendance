namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v18 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Drivers", "NationalCode", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Drivers", "NationalCode", c => c.String(maxLength: 10));
        }
    }
}
