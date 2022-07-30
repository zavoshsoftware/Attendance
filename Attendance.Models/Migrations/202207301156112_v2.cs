namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cards", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cards", "Code", c => c.Int(nullable: false));
        }
    }
}
