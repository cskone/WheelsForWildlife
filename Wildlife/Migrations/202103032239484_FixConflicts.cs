namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixConflicts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DriverLocation_AddressLine1", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_AddressLine2", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_Building", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_City", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_CountryRegion", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_FloorLevel", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_PostalCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "DriverLocation_StateProvince", c => c.String());
            DropColumn("dbo.AspNetUsers", "DriverLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DriverLocation", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "DriverLocation_StateProvince");
            DropColumn("dbo.AspNetUsers", "DriverLocation_PostalCode");
            DropColumn("dbo.AspNetUsers", "DriverLocation_FloorLevel");
            DropColumn("dbo.AspNetUsers", "DriverLocation_CountryRegion");
            DropColumn("dbo.AspNetUsers", "DriverLocation_City");
            DropColumn("dbo.AspNetUsers", "DriverLocation_Building");
            DropColumn("dbo.AspNetUsers", "DriverLocation_AddressLine2");
            DropColumn("dbo.AspNetUsers", "DriverLocation_AddressLine1");
        }
    }
}
