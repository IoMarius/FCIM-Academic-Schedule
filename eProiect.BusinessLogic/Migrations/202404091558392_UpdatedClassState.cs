namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedClassState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "LeadingUser_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Classes", "LeadingUser_Id");
            AddForeignKey("dbo.Classes", "LeadingUser_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Classes", "LeadingUser_Id", "dbo.Users");
            DropIndex("dbo.Classes", new[] { "LeadingUser_Id" });
            DropColumn("dbo.Classes", "LeadingUser_Id");
        }
    }
}
