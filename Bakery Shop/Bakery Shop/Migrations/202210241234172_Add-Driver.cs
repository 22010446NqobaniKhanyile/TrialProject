namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDriver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        UserId = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        TotalDeliveries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Drivers");
        }
    }
}
