namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Required_CareManager_Email : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CareManagers", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CareManagers", "Email", c => c.String());
        }
    }
}
