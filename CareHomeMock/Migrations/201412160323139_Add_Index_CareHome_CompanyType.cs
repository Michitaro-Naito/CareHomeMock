namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Index_CareHome_CompanyType : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CareHomes", "CompanyType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CareHomes", new[] { "CompanyType" });
        }
    }
}
