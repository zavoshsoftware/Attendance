namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V29 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cards", "GroupItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "GroupItemId", c => c.Guid(nullable: false));
        }
    }
}
