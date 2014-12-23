namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareManager_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareManagers", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareManagers", "Order");
        }
    }
}
