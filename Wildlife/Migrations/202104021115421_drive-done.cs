namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drivedone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drives", "DriveDone", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drives", "DriveDone");
        }
    }
}
