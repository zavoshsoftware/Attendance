namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardLoginHistories", "Pleck", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardLoginHistories", "Pleck");
        }
    }
}
