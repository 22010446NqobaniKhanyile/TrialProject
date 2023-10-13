namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedDataAddCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Categoty", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Categoty");
        }
    }
}
