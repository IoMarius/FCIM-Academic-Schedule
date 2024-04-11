namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deleted_ClassToGroup_edited_Something : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupToClasses", "AcademicGroupId", "dbo.AcademicGroups");
            DropForeignKey("dbo.GroupToClasses", "ClassId", "dbo.Classes");
            DropIndex("dbo.GroupToClasses", new[] { "ClassId" });
            DropIndex("dbo.GroupToClasses", new[] { "AcademicGroupId" });
            RenameColumn(table: "dbo.Users", name: "Credentials_Id", newName: "UserCredentialId");
            RenameIndex(table: "dbo.Users", name: "IX_Credentials_Id", newName: "IX_UserCredentialId");
            AddColumn("dbo.AcademicGroups", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "GroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "AcademicGroup_Id", c => c.Int());
            AddColumn("dbo.ClassRooms", "Floor", c => c.Int(nullable: false));
            CreateIndex("dbo.Classes", "AcademicGroup_Id");
            AddForeignKey("dbo.Classes", "AcademicGroup_Id", "dbo.AcademicGroups", "Id");
            DropTable("dbo.GroupToClasses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GroupToClasses",
                c => new
                    {
                        ClassId = c.Int(nullable: false),
                        AcademicGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClassId, t.AcademicGroupId });
            
            DropForeignKey("dbo.Classes", "AcademicGroup_Id", "dbo.AcademicGroups");
            DropIndex("dbo.Classes", new[] { "AcademicGroup_Id" });
            DropColumn("dbo.ClassRooms", "Floor");
            DropColumn("dbo.Classes", "AcademicGroup_Id");
            DropColumn("dbo.Classes", "GroupId");
            DropColumn("dbo.AcademicGroups", "Year");
            RenameIndex(table: "dbo.Users", name: "IX_UserCredentialId", newName: "IX_Credentials_Id");
            RenameColumn(table: "dbo.Users", name: "UserCredentialId", newName: "Credentials_Id");
            CreateIndex("dbo.GroupToClasses", "AcademicGroupId");
            CreateIndex("dbo.GroupToClasses", "ClassId");
            AddForeignKey("dbo.GroupToClasses", "ClassId", "dbo.Classes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupToClasses", "AcademicGroupId", "dbo.AcademicGroups", "Id", cascadeDelete: true);
        }
    }
}
