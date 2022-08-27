namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardLoginHistories", "Load", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.CardLoginHistories", "LoadType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CardLoginHistories", "LoadType", c => c.String());
            DropColumn("dbo.CardLoginHistories", "Load");
        }
    }
}
