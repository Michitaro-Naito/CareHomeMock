namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSurrogateKey_File_MediaFile : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MediaFiles");
            DropPrimaryKey("dbo.Files");
            AddColumn("dbo.MediaFiles", "MediaFileId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Files", "FileId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.MediaFiles", "RowKey", c => c.String());
            AlterColumn("dbo.Files", "RowKey", c => c.String());
            AddPrimaryKey("dbo.MediaFiles", "MediaFileId");
            AddPrimaryKey("dbo.Files", "FileId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Files");
            DropPrimaryKey("dbo.MediaFiles");
            AlterColumn("dbo.Files", "RowKey", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.MediaFiles", "RowKey", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Files", "FileId");
            DropColumn("dbo.MediaFiles", "MediaFileId");
            AddPrimaryKey("dbo.Files", "RowKey");
            AddPrimaryKey("dbo.MediaFiles", "RowKey");
        }
    }
}
