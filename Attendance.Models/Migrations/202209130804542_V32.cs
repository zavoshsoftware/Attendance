namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V32 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Log = c.String(),
                        UserId = c.Guid(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SystemLogs", "UserId", "dbo.Users");
            DropIndex("dbo.SystemLogs", new[] { "UserId" });
            DropTable("dbo.SystemLogs");
        }
    }
}
