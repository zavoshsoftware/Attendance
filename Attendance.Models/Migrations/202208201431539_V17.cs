namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Father", c => c.String());
            DropColumn("dbo.Cards", "Father");
            AlterColumn("dbo.Drivers", "NationalCode", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "Father", c => c.String());
            DropColumn("dbo.Drivers", "Father");
            AlterColumn("dbo.Drivers", "NationalCode", c => c.String());
        }
    }
}
