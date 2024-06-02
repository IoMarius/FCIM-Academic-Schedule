namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class classesConfirm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "IsConfirmed", c => c.Boolean(nullable: false));
           
        }
        
        public override void Down()
        {
           
            DropColumn("dbo.Classes", "IsConfirmed");
        }
    }
}
