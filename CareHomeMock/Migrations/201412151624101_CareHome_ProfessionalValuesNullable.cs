namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareHome_ProfessionalValuesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CareHomes", "介護支援専門員在席人数", c => c.Double());
            AlterColumn("dbo.CareHomes", "介護支援専門員常勤換算", c => c.Double());
            AlterColumn("dbo.CareHomes", "事務員在席人数", c => c.Double());
            AlterColumn("dbo.CareHomes", "事務員常勤換算", c => c.Double());
            AlterColumn("dbo.CareHomes", "その他在席人数", c => c.Double());
            AlterColumn("dbo.CareHomes", "その他常勤換算", c => c.Double());
            AlterColumn("dbo.CareHomes", "全職員在席人数", c => c.Double());
            AlterColumn("dbo.CareHomes", "全職員常勤換算", c => c.Double());
            AlterColumn("dbo.CareHomes", "経験5年以上割合", c => c.Double());
            AlterColumn("dbo.CareHomes", "利用者数", c => c.Double());
            AlterColumn("dbo.CareHomes", "要介護5", c => c.Double());
            AlterColumn("dbo.CareHomes", "要介護4", c => c.Double());
            AlterColumn("dbo.CareHomes", "要介護3", c => c.Double());
            AlterColumn("dbo.CareHomes", "要介護2", c => c.Double());
            AlterColumn("dbo.CareHomes", "要介護1", c => c.Double());
            AlterColumn("dbo.CareHomes", "要支援2", c => c.Double());
            AlterColumn("dbo.CareHomes", "要支援1", c => c.Double());
            AlterColumn("dbo.CareHomes", "自立", c => c.Double());
            AlterColumn("dbo.CareHomes", "利用者の権利擁護", c => c.Double());
            AlterColumn("dbo.CareHomes", "サービスの質の確保", c => c.Double());
            AlterColumn("dbo.CareHomes", "相談苦情等への対応", c => c.Double());
            AlterColumn("dbo.CareHomes", "外部機関等との連携", c => c.Double());
            AlterColumn("dbo.CareHomes", "事業運営管理", c => c.Double());
            AlterColumn("dbo.CareHomes", "安全衛生管理等", c => c.Double());
            AlterColumn("dbo.CareHomes", "従業者の研修等", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CareHomes", "従業者の研修等", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "安全衛生管理等", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "事業運営管理", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "外部機関等との連携", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "相談苦情等への対応", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "サービスの質の確保", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "利用者の権利擁護", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "自立", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要支援1", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要支援2", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要介護1", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要介護2", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要介護3", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要介護4", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "要介護5", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "利用者数", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "経験5年以上割合", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "全職員常勤換算", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "全職員在席人数", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "その他常勤換算", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "その他在席人数", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "事務員常勤換算", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "事務員在席人数", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "介護支援専門員常勤換算", c => c.Double(nullable: false));
            AlterColumn("dbo.CareHomes", "介護支援専門員在席人数", c => c.Double(nullable: false));
        }
    }
}
