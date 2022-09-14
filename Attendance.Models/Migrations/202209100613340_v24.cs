namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExceptionLoggers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ExceptionMessage = c.String(),
                        ExceptionStackTrace = c.String(),
                        RouteName = c.String(),
                        Description = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExceptionLoggers");
        }
    }
}
