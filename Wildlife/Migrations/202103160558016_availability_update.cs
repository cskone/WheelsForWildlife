namespace Wildlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availability_update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Availability", "Day", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Availability", "Day", c => c.String());
        }
    }
}
