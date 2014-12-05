namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareHome_ReviewCount_and_Rating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareHomes", "ReviewCount", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Rating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareHomes", "Rating");
            DropColumn("dbo.CareHomes", "ReviewCount");
        }
    }
}
