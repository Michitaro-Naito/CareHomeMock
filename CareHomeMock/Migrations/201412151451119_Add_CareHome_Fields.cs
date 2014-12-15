namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareHome_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareHomes", "Name", c => c.String(maxLength: 255));
            AddColumn("dbo.CareHomes", "AddressBuilding", c => c.String());
            AddColumn("dbo.CareHomes", "利用者数", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareHomes", "利用者数");
            DropColumn("dbo.CareHomes", "AddressBuilding");
            DropColumn("dbo.CareHomes", "Name");
        }
    }
}
