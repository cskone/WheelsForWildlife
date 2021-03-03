namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapdetailsaddedtodrive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drives", "MapDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drives", "MapDetails");
        }
    }
}
