namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardLoginHistories", "LoadType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardLoginHistories", "LoadType");
        }
    }
}
