namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareManager_ShowReviews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareManagers", "ShowReviews", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareManagers", "ShowReviews");
        }
    }
}
