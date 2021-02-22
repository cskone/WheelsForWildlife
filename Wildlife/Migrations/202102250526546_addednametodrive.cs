namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addednametodrive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drives", "DriveName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drives", "DriveName");
        }
    }
}
