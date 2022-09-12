namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V28 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardGroupItemCards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CardGroupItemId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.CardGroupItems", t => t.CardGroupItemId, cascadeDelete: true)
                .Index(t => t.CardGroupItemId)
                .Index(t => t.CardId);
            
            CreateTable(
                "dbo.CardGroupItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 20),
                        GroupId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardGroups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.CardGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
             
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardGroupItemCards", "CardGroupItemId", "dbo.CardGroupItems");
            DropForeignKey("dbo.CardGroupItems", "GroupId", "dbo.CardGroups");
            DropForeignKey("dbo.CardGroupItemCards", "CardId", "dbo.Cards");
            DropIndex("dbo.CardGroupItems", new[] { "GroupId" });
            DropIndex("dbo.CardGroupItemCards", new[] { "CardId" });
            DropIndex("dbo.CardGroupItemCards", new[] { "CardGroupItemId" }); 
            DropTable("dbo.CardGroups");
            DropTable("dbo.CardGroupItems");
            DropTable("dbo.CardGroupItemCards");
        }
    }
}
