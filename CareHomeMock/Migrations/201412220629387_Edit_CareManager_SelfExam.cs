namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_CareManager_SelfExam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareManagers", "指導管理力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "公平中立力", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareManagers", "公平中立力");
            DropColumn("dbo.CareManagers", "指導管理力");
        }
    }
}
