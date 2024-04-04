namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreAlphaModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Classes", name: "WeekdayId", newName: "WeekDay_Id");
            RenameColumn(table: "dbo.Classes", name: "ClassRoomId", newName: "Classoom_Id");
            RenameIndex(table: "dbo.Classes", name: "IX_ClassRoomId", newName: "IX_Classoom_Id");
            RenameIndex(table: "dbo.Classes", name: "IX_WeekdayId", newName: "IX_WeekDay_Id");
            AddColumn("dbo.Classes", "Discipline_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Disciplines", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Disciplines", "ShortName", c => c.String(nullable: false, maxLength: 15));
            CreateIndex("dbo.Classes", "Discipline_Id");
            AddForeignKey("dbo.Classes", "Discipline_Id", "dbo.Disciplines", "Id", cascadeDelete: true);
            DropColumn("dbo.Classes", "DisciplineID");
            DropColumn("dbo.Disciplines", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Disciplines", "MyProperty", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Classes", "DisciplineID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Classes", "Discipline_Id", "dbo.Disciplines");
            DropIndex("dbo.Classes", new[] { "Discipline_Id" });
            DropColumn("dbo.Disciplines", "ShortName");
            DropColumn("dbo.Disciplines", "Name");
            DropColumn("dbo.Classes", "Discipline_Id");
            RenameIndex(table: "dbo.Classes", name: "IX_WeekDay_Id", newName: "IX_WeekdayId");
            RenameIndex(table: "dbo.Classes", name: "IX_Classoom_Id", newName: "IX_ClassRoomId");
            RenameColumn(table: "dbo.Classes", name: "Classoom_Id", newName: "ClassRoomId");
            RenameColumn(table: "dbo.Classes", name: "WeekDay_Id", newName: "WeekdayId");
        }
    }
}
