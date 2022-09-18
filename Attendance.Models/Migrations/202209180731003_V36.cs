namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V36 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarTypeStatusHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PreviousStatus = c.Boolean(nullable: false),
                        CurrentStatus = c.Boolean(nullable: false),
                        CarTypeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarTypes", t => t.CarTypeId, cascadeDelete: true)
                .Index(t => t.CarTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarTypeStatusHistories", "CarTypeId", "dbo.CarTypes");
            DropIndex("dbo.CarTypeStatusHistories", new[] { "CarTypeId" });
            DropTable("dbo.CarTypeStatusHistories");
        }
    }
}
