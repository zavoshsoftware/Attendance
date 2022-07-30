namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CardLoginHistories", "TotalLoad", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CardLoginHistories", "TotalLoad", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
