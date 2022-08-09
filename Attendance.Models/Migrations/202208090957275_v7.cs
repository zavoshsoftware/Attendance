namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardLoginHistories", "CarId", c => c.Guid());
            AddColumn("dbo.CarTypes", "Brand", c => c.String(maxLength: 100));
            CreateIndex("dbo.CardLoginHistories", "CarId");
            AddForeignKey("dbo.CardLoginHistories", "CarId", "dbo.Cars", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardLoginHistories", "CarId", "dbo.Cars");
            DropIndex("dbo.CardLoginHistories", new[] { "CarId" });
            DropColumn("dbo.CarTypes", "Brand");
            DropColumn("dbo.CardLoginHistories", "CarId");
        }
    }
}
