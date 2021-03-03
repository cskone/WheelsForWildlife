namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_mapdetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drives", "DriveDistance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Drives", "DriveDuration", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Drives", "MapDetails");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drives", "MapDetails", c => c.String());
            DropColumn("dbo.Drives", "DriveDuration");
            DropColumn("dbo.Drives", "DriveDistance");
        }
    }
}
