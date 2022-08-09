namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CardDays", "CardId", "dbo.Cards");
            DropIndex("dbo.CardDays", new[] { "CardId" });
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CarTypeId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 100),
                        Number = c.String(),
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
            
            CreateTable(
                "dbo.CarTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 100),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Cards", "Day", c => c.Int(nullable: false));
            DropTable("dbo.CardDays");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Cars", "CarTypeId", "dbo.CarTypes");
            DropIndex("dbo.Cars", new[] { "CarTypeId" });
            DropColumn("dbo.Cards", "Day");
            DropTable("dbo.CarTypes");
            DropTable("dbo.Cars");
            CreateIndex("dbo.CardDays", "CardId");
            AddForeignKey("dbo.CardDays", "CardId", "dbo.Cards", "Id", cascadeDelete: true);
        }
    }
}
