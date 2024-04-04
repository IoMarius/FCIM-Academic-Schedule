namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreAlphaModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Classes", "Type_Id", c => c.Int(nullable: false));
            AddColumn("dbo.ClassRooms", "classroomName", c => c.String(maxLength: 10));
            CreateIndex("dbo.Classes", "Type_Id");
            AddForeignKey("dbo.Classes", "Type_Id", "dbo.ClassTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Classes", "Type");
            DropColumn("dbo.ClassRooms", "classroomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClassRooms", "classroomId", c => c.String(maxLength: 10));
            AddColumn("dbo.Classes", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.Classes", "Type_Id", "dbo.ClassTypes");
            DropIndex("dbo.Classes", new[] { "Type_Id" });
            DropColumn("dbo.ClassRooms", "classroomName");
            DropColumn("dbo.Classes", "Type_Id");
            DropTable("dbo.ClassTypes");
        }
    }
}
