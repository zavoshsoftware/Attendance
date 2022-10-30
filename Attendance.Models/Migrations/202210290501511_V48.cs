namespace Attendance.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V48 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardGroupItemCards", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cards", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CardLoginHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cars", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CarStatusHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CarTypes", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CarTypeStatusHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.DriverStatusHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoginHistoryTools", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tools", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Units", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CardOwnerHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CardStatusHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Penalties", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Penalty_PenaltyType", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.PenaltyTypes", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.PenaltyReasons", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.WalkingLoginHistories", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CardGroupItems", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.CardGroups", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Configs", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ShiftDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserLogins", "ShiftDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLogins", "ShiftDelete");
            DropColumn("dbo.Users", "ShiftDelete");
            DropColumn("dbo.Configs", "ShiftDelete");
            DropColumn("dbo.CardGroups", "ShiftDelete");
            DropColumn("dbo.CardGroupItems", "ShiftDelete");
            DropColumn("dbo.WalkingLoginHistories", "ShiftDelete");
            DropColumn("dbo.PenaltyReasons", "ShiftDelete");
            DropColumn("dbo.PenaltyTypes", "ShiftDelete");
            DropColumn("dbo.Penalty_PenaltyType", "ShiftDelete");
            DropColumn("dbo.Penalties", "ShiftDelete");
            DropColumn("dbo.CardStatusHistories", "ShiftDelete");
            DropColumn("dbo.CardOwnerHistories", "ShiftDelete");
            DropColumn("dbo.Units", "ShiftDelete");
            DropColumn("dbo.Tools", "ShiftDelete");
            DropColumn("dbo.LoginHistoryTools", "ShiftDelete");
            DropColumn("dbo.DriverStatusHistories", "ShiftDelete");
            DropColumn("dbo.Drivers", "ShiftDelete");
            DropColumn("dbo.CarTypeStatusHistories", "ShiftDelete");
            DropColumn("dbo.CarTypes", "ShiftDelete");
            DropColumn("dbo.CarStatusHistories", "ShiftDelete");
            DropColumn("dbo.Cars", "ShiftDelete");
            DropColumn("dbo.CardLoginHistories", "ShiftDelete");
            DropColumn("dbo.Cards", "ShiftDelete");
            DropColumn("dbo.CardGroupItemCards", "ShiftDelete");
        }
    }
}
