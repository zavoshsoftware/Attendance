namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V45 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExitTools", newName: "Tools");
            CreateTable(
                "dbo.LoginHistoryTools",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CardLoginHistoryId = c.Guid(nullable: false),
                        ToolId = c.Guid(nullable: false),
                        UnitId = c.Guid(nullable: false),
                        Amount = c.Double(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardLoginHistories", t => t.CardLoginHistoryId, cascadeDelete: true)
                .ForeignKey("dbo.Tools", t => t.ToolId, cascadeDelete: true)
                .ForeignKey("dbo.Units", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.CardLoginHistoryId)
                .Index(t => t.ToolId)
                .Index(t => t.UnitId);
            
            AddColumn("dbo.Units", "MyProperty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoginHistoryTools", "UnitId", "dbo.Units");
            DropForeignKey("dbo.LoginHistoryTools", "ToolId", "dbo.Tools");
            DropForeignKey("dbo.LoginHistoryTools", "CardLoginHistoryId", "dbo.CardLoginHistories");
            DropIndex("dbo.LoginHistoryTools", new[] { "UnitId" });
            DropIndex("dbo.LoginHistoryTools", new[] { "ToolId" });
            DropIndex("dbo.LoginHistoryTools", new[] { "CardLoginHistoryId" });
            DropColumn("dbo.Units", "MyProperty");
            DropTable("dbo.LoginHistoryTools");
            RenameTable(name: "dbo.Tools", newName: "ExitTools");
        }
    }
}
