namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availability_stuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Availability", "Dayoftheweek", c => c.Int(nullable: false));
            AddColumn("dbo.Availability", "StartTime", c => c.Double(nullable: false));
            AddColumn("dbo.Availability", "EndTime", c => c.Double(nullable: false));
            DropColumn("dbo.Availability", "Day");
            DropColumn("dbo.Availability", "Start");
            DropColumn("dbo.Availability", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Availability", "End", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Availability", "Start", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Availability", "Day", c => c.Int(nullable: false));
            DropColumn("dbo.Availability", "EndTime");
            DropColumn("dbo.Availability", "StartTime");
            DropColumn("dbo.Availability", "Dayoftheweek");
        }
    }
}
