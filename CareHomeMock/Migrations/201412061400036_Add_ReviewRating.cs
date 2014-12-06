namespace CareHomeMock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReviewRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReviewRatings",
                c => new
                    {
                        ReviewRatingId = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        CareManagerId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewRatingId)
                .ForeignKey("dbo.CareManagers", t => t.CareManagerId, cascadeDelete: true)
                .Index(t => t.CareManagerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewRatings", "CareManagerId", "dbo.CareManagers");
            DropIndex("dbo.ReviewRatings", new[] { "CareManagerId" });
            DropTable("dbo.ReviewRatings");
        }
    }
}
