namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareHome_Longitude_Latitude_Index : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CareHomes", "Longitude");
            CreateIndex("dbo.CareHomes", "Latitude");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CareHomes", new[] { "Latitude" });
            DropIndex("dbo.CareHomes", new[] { "Longitude" });
        }
    }
}
