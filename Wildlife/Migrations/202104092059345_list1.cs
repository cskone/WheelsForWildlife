namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class list1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OptedInDrivers");
            AddColumn("dbo.OptedInDrivers", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.OptedInDrivers", "DriverId", c => c.String());
            AddPrimaryKey("dbo.OptedInDrivers", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OptedInDrivers");
            AlterColumn("dbo.OptedInDrivers", "DriverId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.OptedInDrivers", "Id");
            AddPrimaryKey("dbo.OptedInDrivers", "DriverId");
        }
    }
}
