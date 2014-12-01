namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Deactivated_CareHome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareHomes", "Deactivated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareHomes", "Deactivated");
        }
    }
}
