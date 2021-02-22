namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drives : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drives",
                c => new
                    {
                        DriveId = c.Int(nullable: false, identity: true),
                        StartLocation = c.Int(nullable: false),
                        EndLocation = c.Int(nullable: false),
                        ExtraDetails = c.String(),
                        DriverId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DriveId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drives", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Drives", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Drives");
        }
    }
}
