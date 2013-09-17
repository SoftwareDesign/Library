namespace MMLibrarySystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BookNumber = c.String(),
                        BookInfo_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInfoes", t => t.BookInfo_Id)
                .Index(t => t.BookInfo_Id);
            
            CreateTable(
                "dbo.BookInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Books", new[] { "BookInfo_Id" });
            DropForeignKey("dbo.Books", "BookInfo_Id", "dbo.BookInfoes");
            DropTable("dbo.BookInfoes");
            DropTable("dbo.Books");
        }
    }
}
