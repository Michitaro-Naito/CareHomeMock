namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserHasManyCareHomesAndCareManagers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareHomes", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.CareManagers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.CareHomes", "UserId");
            CreateIndex("dbo.CareManagers", "UserId");
            AddForeignKey("dbo.CareManagers", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CareHomes", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CareHomes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CareManagers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.CareManagers", new[] { "UserId" });
            DropIndex("dbo.CareHomes", new[] { "UserId" });
            DropColumn("dbo.CareManagers", "UserId");
            DropColumn("dbo.CareHomes", "UserId");
        }
    }
}
