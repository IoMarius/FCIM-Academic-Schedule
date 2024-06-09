namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserResetTable_test2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserResetPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 90),
                        ExpireTime = c.DateTime(nullable: false),
                        ResetCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserResetPasswords");
        }
    }
}
