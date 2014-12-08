namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MediaFile_Description : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaFiles", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MediaFiles", "Description");
        }
    }
}
