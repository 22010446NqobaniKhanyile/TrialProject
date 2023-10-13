namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPriceToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductPrice", c => c.String(nullable: false));
        }
    }
}
