namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Index_CareHome_Name_Traits : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CareHomes", "Traits", c => c.String(maxLength: 255));
            CreateIndex("dbo.CareHomes", "Name");
            CreateIndex("dbo.CareHomes", "Traits");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CareHomes", new[] { "Traits" });
            DropIndex("dbo.CareHomes", new[] { "Name" });
            AlterColumn("dbo.CareHomes", "Traits", c => c.String());
        }
    }
}
