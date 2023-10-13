namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomOrders",
                c => new
                    {
                        CustomOrderID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        Zip = c.String(),
                        Phone = c.String(),
                        numberOfServings = c.Int(nullable: false),
                        Flavour = c.String(),
                        Icing = c.String(),
                        Filling = c.String(),
                        CakeType = c.String(),
                        SketchImage = c.String(),
                    })
                .PrimaryKey(t => t.CustomOrderID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomOrders");
        }
    }
}
