namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V46 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WalkingLoginHistories", "CardLoginHistoryId", "dbo.CardLoginHistories");
            DropIndex("dbo.WalkingLoginHistories", new[] { "CardLoginHistoryId" });
            AddColumn("dbo.WalkingLoginHistories", "CardId", c => c.Guid(nullable: false));
            CreateIndex("dbo.WalkingLoginHistories", "CardId");
            AddForeignKey("dbo.WalkingLoginHistories", "CardId", "dbo.Cards", "Id", cascadeDelete: true);
            DropColumn("dbo.WalkingLoginHistories", "CardLoginHistoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WalkingLoginHistories", "CardLoginHistoryId", c => c.Guid());
            DropForeignKey("dbo.WalkingLoginHistories", "CardId", "dbo.Cards");
            DropIndex("dbo.WalkingLoginHistories", new[] { "CardId" });
            DropColumn("dbo.WalkingLoginHistories", "CardId");
            CreateIndex("dbo.WalkingLoginHistories", "CardLoginHistoryId");
            AddForeignKey("dbo.WalkingLoginHistories", "CardLoginHistoryId", "dbo.CardLoginHistories", "Id");
        }
    }
}
