namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropComplexType_CareHome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareHomes", "介護支援専門員在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "介護支援専門員常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "事務員在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "事務員常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "その他在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "その他常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "全職員在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "全職員常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "経験5年以上割合", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要介護5", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要介護4", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要介護3", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要介護2", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要介護1", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要支援2", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "要支援1", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "自立", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "利用者の権利擁護", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "サービスの質の確保", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "相談苦情等への対応", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "外部機関等との連携", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "事業運営管理", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "安全衛生管理等", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "従業者の研修等", c => c.Double(nullable: false));
            DropColumn("dbo.CareHomes", "Parameters_介護支援専門員在席人数");
            DropColumn("dbo.CareHomes", "Parameters_介護支援専門員常勤換算");
            DropColumn("dbo.CareHomes", "Parameters_事務員在席人数");
            DropColumn("dbo.CareHomes", "Parameters_事務員常勤換算");
            DropColumn("dbo.CareHomes", "Parameters_その他在席人数");
            DropColumn("dbo.CareHomes", "Parameters_その他常勤換算");
            DropColumn("dbo.CareHomes", "Parameters_全職員在席人数");
            DropColumn("dbo.CareHomes", "Parameters_全職員常勤換算");
            DropColumn("dbo.CareHomes", "Parameters_経験5年以上割合");
            DropColumn("dbo.CareHomes", "Parameters_要介護5");
            DropColumn("dbo.CareHomes", "Parameters_要介護4");
            DropColumn("dbo.CareHomes", "Parameters_要介護3");
            DropColumn("dbo.CareHomes", "Parameters_要介護2");
            DropColumn("dbo.CareHomes", "Parameters_要介護1");
            DropColumn("dbo.CareHomes", "Parameters_要支援2");
            DropColumn("dbo.CareHomes", "Parameters_要支援1");
            DropColumn("dbo.CareHomes", "Parameters_自立");
            DropColumn("dbo.CareHomes", "Parameters_利用者の権利擁護");
            DropColumn("dbo.CareHomes", "Parameters_サービスの質の確保");
            DropColumn("dbo.CareHomes", "Parameters_相談苦情等への対応");
            DropColumn("dbo.CareHomes", "Parameters_外部機関等との連携");
            DropColumn("dbo.CareHomes", "Parameters_事業運営管理");
            DropColumn("dbo.CareHomes", "Parameters_安全衛生管理等");
            DropColumn("dbo.CareHomes", "Parameters_従業者の研修等");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareHomes", "Parameters_従業者の研修等", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_安全衛生管理等", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_事業運営管理", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_外部機関等との連携", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_相談苦情等への対応", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_サービスの質の確保", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_利用者の権利擁護", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_自立", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要支援1", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要支援2", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要介護1", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要介護2", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要介護3", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要介護4", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_要介護5", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_経験5年以上割合", c => c.Double(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_全職員常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_全職員在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_その他常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_その他在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_事務員常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_事務員在席人数", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_介護支援専門員常勤換算", c => c.Int(nullable: false));
            AddColumn("dbo.CareHomes", "Parameters_介護支援専門員在席人数", c => c.Int(nullable: false));
            DropColumn("dbo.CareHomes", "従業者の研修等");
            DropColumn("dbo.CareHomes", "安全衛生管理等");
            DropColumn("dbo.CareHomes", "事業運営管理");
            DropColumn("dbo.CareHomes", "外部機関等との連携");
            DropColumn("dbo.CareHomes", "相談苦情等への対応");
            DropColumn("dbo.CareHomes", "サービスの質の確保");
            DropColumn("dbo.CareHomes", "利用者の権利擁護");
            DropColumn("dbo.CareHomes", "自立");
            DropColumn("dbo.CareHomes", "要支援1");
            DropColumn("dbo.CareHomes", "要支援2");
            DropColumn("dbo.CareHomes", "要介護1");
            DropColumn("dbo.CareHomes", "要介護2");
            DropColumn("dbo.CareHomes", "要介護3");
            DropColumn("dbo.CareHomes", "要介護4");
            DropColumn("dbo.CareHomes", "要介護5");
            DropColumn("dbo.CareHomes", "経験5年以上割合");
            DropColumn("dbo.CareHomes", "全職員常勤換算");
            DropColumn("dbo.CareHomes", "全職員在席人数");
            DropColumn("dbo.CareHomes", "その他常勤換算");
            DropColumn("dbo.CareHomes", "その他在席人数");
            DropColumn("dbo.CareHomes", "事務員常勤換算");
            DropColumn("dbo.CareHomes", "事務員在席人数");
            DropColumn("dbo.CareHomes", "介護支援専門員常勤換算");
            DropColumn("dbo.CareHomes", "介護支援専門員在席人数");
        }
    }
}
