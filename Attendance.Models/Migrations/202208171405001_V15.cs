namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WalkingLoginHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WalkingLoginHistoryType = c.Int(nullable: false),
                        CardLoginHistoryId = c.Guid(),
                        IsDriver = c.Boolean(nullable: false),
                        IsDriverHelper = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardLoginHistories", t => t.CardLoginHistoryId)
                .Index(t => t.CardLoginHistoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WalkingLoginHistories", "CardLoginHistoryId", "dbo.CardLoginHistories");
            DropIndex("dbo.WalkingLoginHistories", new[] { "CardLoginHistoryId" });
            DropTable("dbo.WalkingLoginHistories");
        }
    }
}
