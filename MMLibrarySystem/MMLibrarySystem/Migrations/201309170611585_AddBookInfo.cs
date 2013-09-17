namespace MMLibrarySystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookInfoes", "Description", c => c.String());
            AddColumn("dbo.BookInfoes", "UserAndTeam", c => c.String());
            AddColumn("dbo.BookInfoes", "Publisher", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookInfoes", "Publisher");
            DropColumn("dbo.BookInfoes", "UserAndTeam");
            DropColumn("dbo.BookInfoes", "Description");
        }
    }
}
