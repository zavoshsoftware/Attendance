namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V37 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarStatusHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PreviousStatus = c.Boolean(nullable: false),
                        CurrentStatus = c.Boolean(nullable: false),
                        CarId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarStatusHistories", "CarId", "dbo.Cars");
            DropIndex("dbo.CarStatusHistories", new[] { "CarId" });
            DropTable("dbo.CarStatusHistories");
        }
    }
}
