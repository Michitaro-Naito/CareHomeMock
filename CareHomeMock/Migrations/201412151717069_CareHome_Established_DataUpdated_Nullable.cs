namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareHome_Established_DataUpdated_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CareHomes", "Established", c => c.DateTime());
            AlterColumn("dbo.CareHomes", "DataUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CareHomes", "DataUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CareHomes", "Established", c => c.DateTime(nullable: false));
        }
    }
}
