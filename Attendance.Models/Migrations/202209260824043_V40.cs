namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardGroupItems", "ParentGroupId", c => c.Guid());
            CreateIndex("dbo.CardGroupItems", "ParentGroupId");
            AddForeignKey("dbo.CardGroupItems", "ParentGroupId", "dbo.CardGroupItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardGroupItems", "ParentGroupId", "dbo.CardGroupItems");
            DropIndex("dbo.CardGroupItems", new[] { "ParentGroupId" });
            DropColumn("dbo.CardGroupItems", "ParentGroupId");
        }
    }
}
