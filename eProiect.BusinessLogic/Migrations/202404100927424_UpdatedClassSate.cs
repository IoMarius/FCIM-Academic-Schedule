namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedClassSate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Classes", "Group_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Classes", "Group_Id");
            AddForeignKey("dbo.Classes", "Group_Id", "dbo.AcademicGroups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Classes", "Group_Id", "dbo.AcademicGroups");
            DropIndex("dbo.Classes", new[] { "Group_Id" });
            DropColumn("dbo.Classes", "Group_Id");
            DropTable("dbo.AcademicGroups");
        }
    }
}
