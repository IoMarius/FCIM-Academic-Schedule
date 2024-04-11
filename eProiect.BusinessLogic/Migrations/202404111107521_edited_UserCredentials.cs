namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edited_UserCredentials : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserCredentials", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserCredentials", new[] { "Email" });
        }
    }
}
