namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Css : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Csses",
                c => new
                    {
                        CssId = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.CssId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Csses");
        }
    }
}
