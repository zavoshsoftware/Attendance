namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V41 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardOwnerHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PreviousDriver = c.Guid(nullable: false),
                        CurrentDriver = c.Guid(nullable: false),
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
            
            AddColumn("dbo.CardGroupItems", "SubTitle", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardOwnerHistories", "CardId", "dbo.Cards");
            DropIndex("dbo.CardOwnerHistories", new[] { "CardId" });
            DropColumn("dbo.CardGroupItems", "SubTitle");
            DropTable("dbo.CardOwnerHistories");
        }
    }
}
