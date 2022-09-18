namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V34 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DriverStatusHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PreviousStatus = c.Boolean(nullable: false),
                        CurrentStatus = c.Boolean(nullable: false),
                        DriverId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverStatusHistories", "DriverId", "dbo.Drivers");
            DropIndex("dbo.DriverStatusHistories", new[] { "DriverId" });
            DropTable("dbo.DriverStatusHistories");
        }
    }
}
