namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availability_update1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Availability", "Start", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Availability", "End", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Availability", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Availability", "Start", c => c.DateTime(nullable: false));
        }
    }
}
