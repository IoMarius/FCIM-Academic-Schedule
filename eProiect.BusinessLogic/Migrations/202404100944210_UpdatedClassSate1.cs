namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedClassSate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassRooms", "Floor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassRooms", "Floor");
        }
    }
}
