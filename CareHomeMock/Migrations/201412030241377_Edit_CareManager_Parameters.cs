namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_CareManager_Parameters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareManagers", "企画立案力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "行動実践力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "関係構築力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "マネジメント力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "医療知識", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "介護知識", c => c.Double(nullable: false));
            DropColumn("dbo.CareManagers", "Parameters_企画立案力");
            DropColumn("dbo.CareManagers", "Parameters_行動実践力");
            DropColumn("dbo.CareManagers", "Parameters_関係構築力");
            DropColumn("dbo.CareManagers", "Parameters_マネジメント力");
            DropColumn("dbo.CareManagers", "Parameters_医療知識");
            DropColumn("dbo.CareManagers", "Parameters_介護知識");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareManagers", "Parameters_介護知識", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "Parameters_医療知識", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "Parameters_マネジメント力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "Parameters_関係構築力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "Parameters_行動実践力", c => c.Double(nullable: false));
            AddColumn("dbo.CareManagers", "Parameters_企画立案力", c => c.Double(nullable: false));
            DropColumn("dbo.CareManagers", "介護知識");
            DropColumn("dbo.CareManagers", "医療知識");
            DropColumn("dbo.CareManagers", "マネジメント力");
            DropColumn("dbo.CareManagers", "関係構築力");
            DropColumn("dbo.CareManagers", "行動実践力");
            DropColumn("dbo.CareManagers", "企画立案力");
        }
    }
}
