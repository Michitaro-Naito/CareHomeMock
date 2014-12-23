namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropIndex_CareHome_Traits : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CareHomes", new[] { "Traits" });
            AlterColumn("dbo.CareHomes", "Traits", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CareHomes", "Traits", c => c.String(maxLength: 255));
            CreateIndex("dbo.CareHomes", "Traits");
        }
    }
}
