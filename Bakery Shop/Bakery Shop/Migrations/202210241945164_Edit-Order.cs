namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "isPreparing", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "isArriving", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "isReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "isDelivered", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "AssignedDriverId", c => c.Int());
            AddColumn("dbo.Orders", "TrackingNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "TrackingNumber");
            DropColumn("dbo.Orders", "AssignedDriverId");
            DropColumn("dbo.Orders", "isDelivered");
            DropColumn("dbo.Orders", "isReceived");
            DropColumn("dbo.Orders", "isArriving");
            DropColumn("dbo.Orders", "isPreparing");
        }
    }
}
