namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V39 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardStatusHistories", "Operator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardStatusHistories", "Operator");
        }
    }
}
