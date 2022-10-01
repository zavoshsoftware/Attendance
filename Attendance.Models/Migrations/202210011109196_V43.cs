namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V43 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Penalty_PenaltyType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PenaltyTypeId = c.Guid(nullable: false),
                        PenaltyId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Penalties", t => t.PenaltyId, cascadeDelete: true)
                .ForeignKey("dbo.PenaltyTypes", t => t.PenaltyTypeId, cascadeDelete: true)
                .Index(t => t.PenaltyTypeId)
                .Index(t => t.PenaltyId);
            
            CreateTable(
                "dbo.PenaltyTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Penalties", "PenaltyType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Penalties", "PenaltyType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Penalty_PenaltyType", "PenaltyTypeId", "dbo.PenaltyTypes");
            DropForeignKey("dbo.Penalty_PenaltyType", "PenaltyId", "dbo.Penalties");
            DropIndex("dbo.Penalty_PenaltyType", new[] { "PenaltyId" });
            DropIndex("dbo.Penalty_PenaltyType", new[] { "PenaltyTypeId" });
            DropTable("dbo.PenaltyTypes");
            DropTable("dbo.Penalty_PenaltyType");
        }
    }
}
