namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverPhone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "Phone");
        }
    }
}
