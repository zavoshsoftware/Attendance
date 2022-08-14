namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "FullName", c => c.String());
            DropColumn("dbo.Cards", "Father");
            DropColumn("dbo.Drivers", "FirstName");
            DropColumn("dbo.Drivers", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drivers", "LastName", c => c.String());
            AddColumn("dbo.Drivers", "FirstName", c => c.String());
            AddColumn("dbo.Cards", "Father", c => c.String());
            DropColumn("dbo.Drivers", "FullName");
        }
    }
}
