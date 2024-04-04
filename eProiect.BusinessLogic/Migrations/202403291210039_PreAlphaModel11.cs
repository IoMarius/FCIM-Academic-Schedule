namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreAlphaModel11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDisciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Discipline_Id = c.Int(nullable: false),
                        Type_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.Discipline_Id, cascadeDelete: true)
                .ForeignKey("dbo.ClassTypes", t => t.Type_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Discipline_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDisciplines", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserDisciplines", "Type_Id", "dbo.ClassTypes");
            DropForeignKey("dbo.UserDisciplines", "Discipline_Id", "dbo.Disciplines");
            DropIndex("dbo.UserDisciplines", new[] { "User_Id" });
            DropIndex("dbo.UserDisciplines", new[] { "Type_Id" });
            DropIndex("dbo.UserDisciplines", new[] { "Discipline_Id" });
            DropTable("dbo.UserDisciplines");
        }
    }
}
