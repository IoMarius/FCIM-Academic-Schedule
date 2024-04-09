namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202404081116514_initialState : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserCredentials", "Password", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserCredentials", "Password", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
