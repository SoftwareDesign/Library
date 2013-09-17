namespace MMLibrarySystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "NEtPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "PurcaseDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Books", "RequestedBy", c => c.String());
            AddColumn("dbo.Books", "PurchaseUrl", c => c.String());
            AddColumn("dbo.BookInfoes", "Supplier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookInfoes", "Supplier");
            DropColumn("dbo.Books", "PurchaseUrl");
            DropColumn("dbo.Books", "RequestedBy");
            DropColumn("dbo.Books", "PurcaseDate");
            DropColumn("dbo.Books", "NEtPrice");
        }
    }
}
