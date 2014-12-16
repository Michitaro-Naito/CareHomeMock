namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Index_CareHome_Established_ReviewCount_Rating : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CareHomes", "Established");
            CreateIndex("dbo.CareHomes", "ReviewCount");
            CreateIndex("dbo.CareHomes", "Rating");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CareHomes", new[] { "Rating" });
            DropIndex("dbo.CareHomes", new[] { "ReviewCount" });
            DropIndex("dbo.CareHomes", new[] { "Established" });
        }
    }
}
