namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Otp_VerificationCode_Unique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Otps", "VerificationCode", c => c.String(nullable: false, maxLength: 4));
            CreateIndex("dbo.Otps", "VerificationCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Otps", new[] { "VerificationCode" });
            AlterColumn("dbo.Otps", "VerificationCode", c => c.String());
        }
    }
}
