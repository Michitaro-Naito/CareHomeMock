namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        CareHomeId = c.Int(),
                        Email = c.String(),
                        Name = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.CareHomes", t => t.CareHomeId)
                .Index(t => t.CareHomeId);
            
            CreateTable(
                "dbo.CareHomes",
                c => new
                    {
                        CareHomeId = c.Int(nullable: false, identity: true),
                        CityCode = c.Int(nullable: false),
                        Zip = c.String(),
                        Address = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        WebsiteUrl = c.String(),
                        Established = c.DateTime(nullable: false),
                        CompanyType = c.Int(nullable: false),
                        CompanyName = c.String(),
                        ChiefName = c.String(),
                        ChiefJobTitle = c.String(),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        DataUpdated = c.DateTime(nullable: false),
                        Parameters_介護支援専門員在席人数 = c.Int(nullable: false),
                        Parameters_介護支援専門員常勤換算 = c.Int(nullable: false),
                        Parameters_事務員在席人数 = c.Int(nullable: false),
                        Parameters_事務員常勤換算 = c.Int(nullable: false),
                        Parameters_その他在席人数 = c.Int(nullable: false),
                        Parameters_その他常勤換算 = c.Int(nullable: false),
                        Parameters_全職員在席人数 = c.Int(nullable: false),
                        Parameters_全職員常勤換算 = c.Int(nullable: false),
                        Parameters_経験5年以上割合 = c.Double(nullable: false),
                        Parameters_要介護5 = c.Double(nullable: false),
                        Parameters_要介護4 = c.Double(nullable: false),
                        Parameters_要介護3 = c.Double(nullable: false),
                        Parameters_要介護2 = c.Double(nullable: false),
                        Parameters_要介護1 = c.Double(nullable: false),
                        Parameters_要支援2 = c.Double(nullable: false),
                        Parameters_要支援1 = c.Double(nullable: false),
                        Parameters_自立 = c.Double(nullable: false),
                        Parameters_利用者の権利擁護 = c.Double(nullable: false),
                        Parameters_サービスの質の確保 = c.Double(nullable: false),
                        Parameters_相談苦情等への対応 = c.Double(nullable: false),
                        Parameters_外部機関等との連携 = c.Double(nullable: false),
                        Parameters_事業運営管理 = c.Double(nullable: false),
                        Parameters_安全衛生管理等 = c.Double(nullable: false),
                        Parameters_従業者の研修等 = c.Double(nullable: false),
                        MediaFileDataId = c.String(),
                        Region = c.String(),
                        Traits = c.String(),
                        Messages = c.String(),
                        Cluster_ClusterId = c.Int(),
                    })
                .PrimaryKey(t => t.CareHomeId)
                .ForeignKey("dbo.Areas", t => t.CityCode, cascadeDelete: true)
                .ForeignKey("dbo.Clusters", t => t.Cluster_ClusterId)
                .Index(t => t.CityCode)
                .Index(t => t.Cluster_ClusterId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        CityCode = c.Int(nullable: false, identity: true),
                        PrefectureCode = c.Int(nullable: false),
                        PrefectureName = c.String(),
                        CityName = c.String(),
                        ZoneCode = c.Int(nullable: false),
                        ZoneName = c.String(),
                        ZonePopulation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CityCode);
            
            CreateTable(
                "dbo.CareManagers",
                c => new
                    {
                        CareManagerId = c.Int(nullable: false, identity: true),
                        CareHomeId = c.Int(nullable: false),
                        MediaFileDataId = c.String(),
                        Name = c.String(),
                        Gender = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Licensed = c.DateTime(nullable: false),
                        Licenses = c.String(),
                        CurrentPatients = c.Int(nullable: false),
                        AllowNewPatient = c.Boolean(nullable: false),
                        Career = c.String(),
                        Messages = c.String(),
                        BlogUrls = c.String(),
                        Parameters_企画立案力 = c.Double(nullable: false),
                        Parameters_行動実践力 = c.Double(nullable: false),
                        Parameters_関係構築力 = c.Double(nullable: false),
                        Parameters_マネジメント力 = c.Double(nullable: false),
                        Parameters_医療知識 = c.Double(nullable: false),
                        Parameters_介護知識 = c.Double(nullable: false),
                        TotalRating = c.Double(nullable: false),
                        ReviewsCount = c.Int(nullable: false),
                        Rating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CareManagerId)
                .ForeignKey("dbo.CareHomes", t => t.CareHomeId, cascadeDelete: true)
                .Index(t => t.CareHomeId);
            
            CreateTable(
                "dbo.EmailVerifications",
                c => new
                    {
                        EmailVerificationId = c.Int(nullable: false, identity: true),
                        Expires = c.DateTime(nullable: false),
                        VerificationCode = c.String(),
                        CareManagerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmailVerificationId)
                .ForeignKey("dbo.CareManagers", t => t.CareManagerId, cascadeDelete: true)
                .Index(t => t.CareManagerId);
            
            CreateTable(
                "dbo.Otps",
                c => new
                    {
                        OtpId = c.Int(nullable: false, identity: true),
                        Expires = c.DateTime(nullable: false),
                        VerificationCode = c.String(),
                        PatientType = c.Int(nullable: false),
                        CareManagerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OtpId)
                .ForeignKey("dbo.CareManagers", t => t.CareManagerId, cascadeDelete: true)
                .Index(t => t.CareManagerId);
            
            CreateTable(
                "dbo.MediaFiles",
                c => new
                    {
                        RowKey = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CareHomeId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RowKey)
                .ForeignKey("dbo.CareHomes", t => t.CareHomeId, cascadeDelete: true)
                .Index(t => t.CareHomeId);
            
            CreateTable(
                "dbo.Clusters",
                c => new
                    {
                        ClusterId = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ClusterId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        RowKey = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RowKey);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StaticPages",
                c => new
                    {
                        StaticPageId = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(),
                        Html = c.String(),
                    })
                .PrimaryKey(t => t.StaticPageId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CareHomes", "Cluster_ClusterId", "dbo.Clusters");
            DropForeignKey("dbo.Applications", "CareHomeId", "dbo.CareHomes");
            DropForeignKey("dbo.MediaFiles", "CareHomeId", "dbo.CareHomes");
            DropForeignKey("dbo.Otps", "CareManagerId", "dbo.CareManagers");
            DropForeignKey("dbo.EmailVerifications", "CareManagerId", "dbo.CareManagers");
            DropForeignKey("dbo.CareManagers", "CareHomeId", "dbo.CareHomes");
            DropForeignKey("dbo.CareHomes", "CityCode", "dbo.Areas");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.MediaFiles", new[] { "CareHomeId" });
            DropIndex("dbo.Otps", new[] { "CareManagerId" });
            DropIndex("dbo.EmailVerifications", new[] { "CareManagerId" });
            DropIndex("dbo.CareManagers", new[] { "CareHomeId" });
            DropIndex("dbo.CareHomes", new[] { "Cluster_ClusterId" });
            DropIndex("dbo.CareHomes", new[] { "CityCode" });
            DropIndex("dbo.Applications", new[] { "CareHomeId" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StaticPages");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Files");
            DropTable("dbo.Clusters");
            DropTable("dbo.MediaFiles");
            DropTable("dbo.Otps");
            DropTable("dbo.EmailVerifications");
            DropTable("dbo.CareManagers");
            DropTable("dbo.Areas");
            DropTable("dbo.CareHomes");
            DropTable("dbo.Applications");
        }
    }
}
