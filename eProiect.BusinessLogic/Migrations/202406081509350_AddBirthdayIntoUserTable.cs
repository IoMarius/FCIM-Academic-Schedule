namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBirthdayIntoUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Birthday", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Birthday");
        }
    }
}
