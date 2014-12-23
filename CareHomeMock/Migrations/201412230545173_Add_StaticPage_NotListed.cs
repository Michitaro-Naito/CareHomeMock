namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_StaticPage_NotListed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaticPages", "NotListed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaticPages", "NotListed");
        }
    }
}
