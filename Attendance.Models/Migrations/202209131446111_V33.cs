namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PenaltyReasons",
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
            
            AddColumn("dbo.Penalties", "ReasonId", c => c.Guid());
            CreateIndex("dbo.Penalties", "ReasonId");
            AddForeignKey("dbo.Penalties", "ReasonId", "dbo.PenaltyReasons", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Penalties", "ReasonId", "dbo.PenaltyReasons");
            DropIndex("dbo.Penalties", new[] { "ReasonId" });
            DropColumn("dbo.Penalties", "ReasonId");
            DropTable("dbo.PenaltyReasons");
        }
    }
}
