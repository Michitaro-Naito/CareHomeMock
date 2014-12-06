namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Otp_ReviewerType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Otps", "ReviewerType", c => c.Int(nullable: false));
            DropColumn("dbo.Otps", "PatientType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Otps", "PatientType", c => c.Int(nullable: false));
            DropColumn("dbo.Otps", "ReviewerType");
        }
    }
}
