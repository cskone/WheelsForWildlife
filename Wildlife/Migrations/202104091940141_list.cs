namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class list : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OptedInDrivers",
                c => new
                    {
                        DriverId = c.String(nullable: false, maxLength: 128),
                        Drive_DriveId = c.Int(),
                    })
                .PrimaryKey(t => t.DriverId)
                .ForeignKey("dbo.Drives", t => t.Drive_DriveId)
                .Index(t => t.Drive_DriveId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptedInDrivers", "Drive_DriveId", "dbo.Drives");
            DropIndex("dbo.OptedInDrivers", new[] { "Drive_DriveId" });
            DropTable("dbo.OptedInDrivers");
        }
    }
}
