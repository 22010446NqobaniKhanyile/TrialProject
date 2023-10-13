namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        OrderNo = c.String(nullable: false),
                        ZipCode = c.String(),
                        CustomerId = c.String(),
                        CustomerEmail = c.String(),
                        CustomerPhone = c.String(),
                        OrderDate = c.DateTime(),
                        Address = c.String(),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
