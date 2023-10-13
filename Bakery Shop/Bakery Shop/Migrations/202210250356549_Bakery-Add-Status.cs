namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BakeryAddStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Status");
        }
    }
}
