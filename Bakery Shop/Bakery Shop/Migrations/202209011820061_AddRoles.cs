namespace Bakery_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoles : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'eb284c38-a56a-45bb-bac4-8b9777815ebb', N'Admin')");
            Sql(@"INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Telephone]) VALUES (N'471516c9-e96d-4f7f-9ccd-bf08d51be931', N'admin@gmail.com', 0, N'AHKV+oLJSXNLz8I0dDBUqaa57kWXSUr1ZcaXMGXBbsW185B5yAp3MJRa2xK+S/ghEA==', N'a9e0926e-28bc-4b73-9f7f-35757cbf47bb', NULL, 0, 0, NULL, 1, 0, N'admin@gmail.com', NULL)");
            Sql(@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'471516c9-e96d-4f7f-9ccd-bf08d51be931', N'eb284c38-a56a-45bb-bac4-8b9777815ebb')");
        }
        
        public override void Down()
        {
        }
    }
}
