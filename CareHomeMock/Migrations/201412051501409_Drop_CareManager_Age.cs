namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drop_CareManager_Age : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CareManagers", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareManagers", "Age", c => c.Int(nullable: false));
        }
    }
}
