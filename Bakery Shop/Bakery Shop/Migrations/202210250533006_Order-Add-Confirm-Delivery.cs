namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderAddConfirmDelivery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsDeliveryConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsDeliveryConfirmed");
        }
    }
}
