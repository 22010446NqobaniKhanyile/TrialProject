namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedDataRenameCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Category", c => c.String());
            DropColumn("dbo.Products", "Categoty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Categoty", c => c.String());
            DropColumn("dbo.Products", "Category");
        }
    }
}
