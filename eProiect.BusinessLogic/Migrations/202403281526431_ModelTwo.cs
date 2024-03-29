namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelTwo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Credentials_Id", "dbo.UserCredentials");
            DropIndex("dbo.Users", new[] { "Credentials_Id" });
            AlterColumn("dbo.Users", "Credentials_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "Credentials_Id");
            AddForeignKey("dbo.Users", "Credentials_Id", "dbo.UserCredentials", "Id", cascadeDelete: true);
            DropColumn("dbo.Users", "CredentialId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CredentialId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "Credentials_Id", "dbo.UserCredentials");
            DropIndex("dbo.Users", new[] { "Credentials_Id" });
            AlterColumn("dbo.Users", "Credentials_Id", c => c.Int());
            CreateIndex("dbo.Users", "Credentials_Id");
            AddForeignKey("dbo.Users", "Credentials_Id", "dbo.UserCredentials", "Id");
        }
    }
}
