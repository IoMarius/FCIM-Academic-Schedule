namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialState : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Frequency = c.Int(nullable: false),
                        Classoom_Id = c.Int(nullable: false),
                        Discipline_Id = c.Int(nullable: false),
                        Type_Id = c.Int(nullable: false),
                        WeekDay_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassRooms", t => t.Classoom_Id, cascadeDelete: true)
                .ForeignKey("dbo.Disciplines", t => t.Discipline_Id, cascadeDelete: true)
                .ForeignKey("dbo.ClassTypes", t => t.Type_Id, cascadeDelete: true)
                .ForeignKey("dbo.WeekDays", t => t.WeekDay_Id, cascadeDelete: true)
                .Index(t => t.Classoom_Id)
                .Index(t => t.Discipline_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.WeekDay_Id);
            
            CreateTable(
                "dbo.ClassRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassroomName = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WeekDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 8),
                        ShortName = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCredentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Surname = c.String(nullable: false, maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        LastIp = c.String(maxLength: 30),
                        Level = c.Int(nullable: false),
                        Credentials_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCredentials", t => t.Credentials_Id, cascadeDelete: true)
                .Index(t => t.Credentials_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDisciplines", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Credentials_Id", "dbo.UserCredentials");
            DropForeignKey("dbo.UserDisciplines", "Type_Id", "dbo.ClassTypes");
            DropForeignKey("dbo.UserDisciplines", "Discipline_Id", "dbo.Disciplines");
            DropForeignKey("dbo.Classes", "WeekDay_Id", "dbo.WeekDays");
            DropForeignKey("dbo.Classes", "Type_Id", "dbo.ClassTypes");
            DropForeignKey("dbo.Classes", "Discipline_Id", "dbo.Disciplines");
            DropForeignKey("dbo.Classes", "Classoom_Id", "dbo.ClassRooms");
            DropIndex("dbo.Users", new[] { "Credentials_Id" });
            DropIndex("dbo.UserDisciplines", new[] { "User_Id" });
            DropIndex("dbo.UserDisciplines", new[] { "Type_Id" });
            DropIndex("dbo.UserDisciplines", new[] { "Discipline_Id" });
            DropIndex("dbo.Classes", new[] { "WeekDay_Id" });
            DropIndex("dbo.Classes", new[] { "Type_Id" });
            DropIndex("dbo.Classes", new[] { "Discipline_Id" });
            DropIndex("dbo.Classes", new[] { "Classoom_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.UserDisciplines");
            DropTable("dbo.UserCredentials");
            DropTable("dbo.WeekDays");
            DropTable("dbo.ClassTypes");
            DropTable("dbo.Disciplines");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.Classes");
        }
    }
}
