namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserID = c.Int(nullable: false),
                        VehicleMake = c.String(),
                        VehicleModel = c.String(),
                        DriveCount = c.Int(nullable: false),
                        DriveHours = c.Double(nullable: false),
                        AnimalCount = c.Int(nullable: false),
                        DriverLocation_AddressLine1 = c.String(),
                        DriverLocation_AddressLine2 = c.String(),
                        DriverLocation_Building = c.String(),
                        DriverLocation_City = c.String(),
                        DriverLocation_CountryRegion = c.String(),
                        DriverLocation_FloorLevel = c.String(),
                        DriverLocation_PostalCode = c.String(),
                        DriverLocation_StateProvince = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Availability",
                c => new
                    {
                        SlotId = c.Int(nullable: false, identity: true),
                        DriverId = c.String(nullable: false),
                        Dayoftheweek = c.Int(nullable: false),
                        StartTime = c.Double(nullable: false),
                        EndTime = c.Double(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SlotId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Drives",
                c => new
                    {
                        DriveId = c.Int(nullable: false, identity: true),
                        DriveName = c.String(nullable: false),
                        StartLocation_AddressLine1 = c.String(),
                        StartLocation_AddressLine2 = c.String(),
                        StartLocation_Building = c.String(),
                        StartLocation_City = c.String(),
                        StartLocation_CountryRegion = c.String(),
                        StartLocation_FloorLevel = c.String(),
                        StartLocation_PostalCode = c.String(),
                        StartLocation_StateProvince = c.String(),
                        EndLocation_AddressLine1 = c.String(),
                        EndLocation_AddressLine2 = c.String(),
                        EndLocation_Building = c.String(),
                        EndLocation_City = c.String(),
                        EndLocation_CountryRegion = c.String(),
                        EndLocation_FloorLevel = c.String(),
                        EndLocation_PostalCode = c.String(),
                        EndLocation_StateProvince = c.String(),
                        ExtraDetails = c.String(),
                        DriverId = c.String(),
                        DriveDistance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DriveDuration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DriveDone = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DriveId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.OptedInDrivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriverId = c.String(),
                        Drive_DriveId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drives", t => t.Drive_DriveId)
                .Index(t => t.Drive_DriveId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Drives", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OptedInDrivers", "Drive_DriveId", "dbo.Drives");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Availability", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.OptedInDrivers", new[] { "Drive_DriveId" });
            DropIndex("dbo.Drives", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Availability", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.OptedInDrivers");
            DropTable("dbo.Drives");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Availability");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
