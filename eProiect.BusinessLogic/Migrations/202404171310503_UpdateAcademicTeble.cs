namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAcademicTeble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassRooms", "ClassroomName", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.AcademicGroups", "Name", unique: true);
            CreateIndex("dbo.ClassRooms", "ClassroomName", unique: true);
            CreateIndex("dbo.Disciplines", "Name", unique: true);
            CreateIndex("dbo.Disciplines", "ShortName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Disciplines", new[] { "ShortName" });
            DropIndex("dbo.Disciplines", new[] { "Name" });
            DropIndex("dbo.ClassRooms", new[] { "ClassroomName" });
            DropIndex("dbo.AcademicGroups", new[] { "Name" });
            AlterColumn("dbo.ClassRooms", "ClassroomName", c => c.String(maxLength: 10));
        }
    }
}
