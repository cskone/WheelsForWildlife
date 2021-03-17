namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availability : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Availability",
                c => new
                    {
                        SlotId = c.Int(nullable: false, identity: true),
                        DriverId = c.String(nullable: false),
                        Day = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SlotId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Availability", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Availability", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Availability");
        }
    }
}
