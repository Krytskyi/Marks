namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Points", "Altitude", c => c.Double(nullable: false));
            AddColumn("dbo.Points", "SymbolRow", c => c.String());
            DropColumn("dbo.Points", "Accuracy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Points", "Accuracy", c => c.String());
            DropColumn("dbo.Points", "SymbolRow");
            DropColumn("dbo.Points", "Altitude");
        }
    }
}
