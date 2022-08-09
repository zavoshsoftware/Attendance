namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CardLoginHistories", "Pleck");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CardLoginHistories", "Pleck", c => c.String());
        }
    }
}
