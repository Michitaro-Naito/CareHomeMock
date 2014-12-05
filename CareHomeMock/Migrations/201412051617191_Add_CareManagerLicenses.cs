namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareManagerLicenses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CareManagerLicenses",
                c => new
                    {
                        CareManagerLicensesId = c.Int(nullable: false, identity: true),
                        CareManagerId = c.Int(nullable: false),
                        LicenseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CareManagerLicensesId)
                .ForeignKey("dbo.CareManagers", t => t.CareManagerId, cascadeDelete: true)
                .ForeignKey("dbo.Licenses", t => t.LicenseId, cascadeDelete: true)
                .Index(t => t.CareManagerId)
                .Index(t => t.LicenseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CareManagerLicenses", "LicenseId", "dbo.Licenses");
            DropForeignKey("dbo.CareManagerLicenses", "CareManagerId", "dbo.CareManagers");
            DropIndex("dbo.CareManagerLicenses", new[] { "LicenseId" });
            DropIndex("dbo.CareManagerLicenses", new[] { "CareManagerId" });
            DropTable("dbo.CareManagerLicenses");
        }
    }
}
