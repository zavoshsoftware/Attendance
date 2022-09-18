namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarTypes", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarTypes", "Code");
        }
    }
}
