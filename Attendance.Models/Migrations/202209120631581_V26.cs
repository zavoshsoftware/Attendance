namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V26 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardStatusHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PreviousStatus = c.Boolean(nullable: false),
                        CurrentStatus = c.Boolean(nullable: false),
                        CardId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.CardId, cascadeDelete: true)
                .Index(t => t.CardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardStatusHistories", "CardId", "dbo.Cards");
            DropIndex("dbo.CardStatusHistories", new[] { "CardId" });
            DropTable("dbo.CardStatusHistories");
        }
    }
}
