namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drivechanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Drives", "DriveName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Drives", "DriveName", c => c.String());
        }
    }
}
