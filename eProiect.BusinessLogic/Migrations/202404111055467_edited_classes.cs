namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edited_classes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classes", "AcademicGroup_Id", "dbo.AcademicGroups");
            DropIndex("dbo.Classes", new[] { "AcademicGroup_Id" });
            RenameColumn(table: "dbo.Classes", name: "AcademicGroup_Id", newName: "AcademicGroupId");
            AlterColumn("dbo.Classes", "AcademicGroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Classes", "AcademicGroupId");
            AddForeignKey("dbo.Classes", "AcademicGroupId", "dbo.AcademicGroups", "Id", cascadeDelete: true);
            DropColumn("dbo.Classes", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "GroupId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Classes", "AcademicGroupId", "dbo.AcademicGroups");
            DropIndex("dbo.Classes", new[] { "AcademicGroupId" });
            AlterColumn("dbo.Classes", "AcademicGroupId", c => c.Int());
            RenameColumn(table: "dbo.Classes", name: "AcademicGroupId", newName: "AcademicGroup_Id");
            CreateIndex("dbo.Classes", "AcademicGroup_Id");
            AddForeignKey("dbo.Classes", "AcademicGroup_Id", "dbo.AcademicGroups", "Id");
        }
    }
}
