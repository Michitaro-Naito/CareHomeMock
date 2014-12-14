namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_CareManager_stringLicenses : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CareManagers", "Licenses");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareManagers", "Licenses", c => c.String());
        }
    }
}
