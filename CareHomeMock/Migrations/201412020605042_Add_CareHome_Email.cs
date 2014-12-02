namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CareHome_Email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareHomes", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareHomes", "Email");
        }
    }
}
