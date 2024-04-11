namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedClassSate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AcademicGroups", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AcademicGroups", "Year");
        }
    }
}
