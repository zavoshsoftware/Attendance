namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardDays",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WeekDays = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.CardLoginHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LoginDate = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        CardId = c.Guid(),
                        ExitDate = c.DateTime(),
                        DriverName = c.String(),
                        DriverHelperName = c.String(),
                        CarNumber = c.String(),
                        TotalLoad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.CardId)
                .Index(t => t.CardId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        CellNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                        Driver_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.Driver_Id)
                .Index(t => t.Driver_Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(),
                        IsSuccessLogin = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CellNum = c.String(nullable: false, maxLength: 20),
                        Password = c.String(maxLength: 150),
                        FullName = c.String(maxLength: 250),
                        Email = c.String(),
                        SecurityRole = c.String(),
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
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Cards", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Drivers", "Driver_Id", "dbo.Drivers");
            DropForeignKey("dbo.CardLoginHistories", "CardId", "dbo.Cards");
            DropForeignKey("dbo.CardDays", "CardId", "dbo.Cards");
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.Drivers", new[] { "Driver_Id" });
            DropIndex("dbo.CardLoginHistories", new[] { "CardId" });
            DropIndex("dbo.Cards", new[] { "DriverId" });
            DropIndex("dbo.CardDays", new[] { "CardId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Drivers");
            DropTable("dbo.CardLoginHistories");
            DropTable("dbo.Cards");
            DropTable("dbo.CardDays");
        }
    }
}
