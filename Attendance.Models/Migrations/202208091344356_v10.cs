namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardLoginHistories", "AssistanceName", c => c.String());
            AddColumn("dbo.CardLoginHistories", "AssistanceLastName", c => c.String());
            AddColumn("dbo.CardLoginHistories", "AssistanceNationalCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardLoginHistories", "AssistanceNationalCode");
            DropColumn("dbo.CardLoginHistories", "AssistanceLastName");
            DropColumn("dbo.CardLoginHistories", "AssistanceName");
        }
    }
}
