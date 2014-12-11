namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_EmailVerification_Email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailVerifications", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailVerifications", "Email");
        }
    }
}
