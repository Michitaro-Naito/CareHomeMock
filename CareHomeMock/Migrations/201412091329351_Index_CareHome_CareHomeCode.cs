namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Index_CareHome_CareHomeCode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CareHomes", "CareHomeCode", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.CareHomes", "CareHomeCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CareHomes", new[] { "CareHomeCode" });
            AlterColumn("dbo.CareHomes", "CareHomeCode", c => c.String());
        }
    }
}
