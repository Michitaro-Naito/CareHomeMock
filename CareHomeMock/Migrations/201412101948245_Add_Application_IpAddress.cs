namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Application_IpAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "IpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "IpAddress");
        }
    }
}
