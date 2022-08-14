namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cards", "Father", c => c.String());
            AddColumn("dbo.Drivers", "FirstName", c => c.String());
            AddColumn("dbo.Drivers", "LastName", c => c.String());
            DropColumn("dbo.Drivers", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drivers", "FullName", c => c.String());
            DropColumn("dbo.Drivers", "LastName");
            DropColumn("dbo.Drivers", "FirstName");
            DropColumn("dbo.Cards", "Father");
        }
    }
}
