namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V47 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LoginHistoryTools", "ToolId", "dbo.Tools");
            DropForeignKey("dbo.LoginHistoryTools", "UnitId", "dbo.Units");
            DropIndex("dbo.LoginHistoryTools", new[] { "ToolId" });
            DropIndex("dbo.LoginHistoryTools", new[] { "UnitId" });
            AlterColumn("dbo.LoginHistoryTools", "ToolId", c => c.Guid());
            AlterColumn("dbo.LoginHistoryTools", "UnitId", c => c.Guid());
            AlterColumn("dbo.LoginHistoryTools", "Amount", c => c.Double());
            CreateIndex("dbo.LoginHistoryTools", "ToolId");
            CreateIndex("dbo.LoginHistoryTools", "UnitId");
            AddForeignKey("dbo.LoginHistoryTools", "ToolId", "dbo.Tools", "Id");
            AddForeignKey("dbo.LoginHistoryTools", "UnitId", "dbo.Units", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoginHistoryTools", "UnitId", "dbo.Units");
            DropForeignKey("dbo.LoginHistoryTools", "ToolId", "dbo.Tools");
            DropIndex("dbo.LoginHistoryTools", new[] { "UnitId" });
            DropIndex("dbo.LoginHistoryTools", new[] { "ToolId" });
            AlterColumn("dbo.LoginHistoryTools", "Amount", c => c.Double(nullable: false));
            AlterColumn("dbo.LoginHistoryTools", "UnitId", c => c.Guid(nullable: false));
            AlterColumn("dbo.LoginHistoryTools", "ToolId", c => c.Guid(nullable: false));
            CreateIndex("dbo.LoginHistoryTools", "UnitId");
            CreateIndex("dbo.LoginHistoryTools", "ToolId");
            AddForeignKey("dbo.LoginHistoryTools", "UnitId", "dbo.Units", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LoginHistoryTools", "ToolId", "dbo.Tools", "Id", cascadeDelete: true);
        }
    }
}
