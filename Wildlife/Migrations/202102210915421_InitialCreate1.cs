namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "VehicleMake", c => c.String());
            AddColumn("dbo.AspNetUsers", "VehicleModel", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriveCount", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "DriveHours", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "AnimalCount", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "DriverLocation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DriverLocation");
            DropColumn("dbo.AspNetUsers", "AnimalCount");
            DropColumn("dbo.AspNetUsers", "DriveHours");
            DropColumn("dbo.AspNetUsers", "DriveCount");
            DropColumn("dbo.AspNetUsers", "VehicleModel");
            DropColumn("dbo.AspNetUsers", "VehicleMake");
            DropColumn("dbo.AspNetUsers", "UserID");
        }
    }
}
