namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MediaFile_YoutubeUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaFiles", "YoutubeUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MediaFiles", "YoutubeUrl");
        }
    }
}
