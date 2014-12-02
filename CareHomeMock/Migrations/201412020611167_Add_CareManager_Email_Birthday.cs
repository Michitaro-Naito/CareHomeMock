namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareManager_Email_Birthday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareManagers", "Email", c => c.String());
            AddColumn("dbo.CareManagers", "Birthday", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareManagers", "Birthday");
            DropColumn("dbo.CareManagers", "Email");
        }
    }
}
