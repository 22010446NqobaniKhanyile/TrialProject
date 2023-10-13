namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedDatabaseUpdateProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "Category_CategoryId");
            AlterColumn("dbo.Products", "Category_CategoryId", c => c.Int());
            CreateIndex("dbo.Products", "Category_CategoryId");
            AddForeignKey("dbo.Products", "Category_CategoryId", "dbo.Categories", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Category_CategoryId" });
            AlterColumn("dbo.Products", "Category_CategoryId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Products", name: "Category_CategoryId", newName: "CategoryId");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
        }
    }
}
