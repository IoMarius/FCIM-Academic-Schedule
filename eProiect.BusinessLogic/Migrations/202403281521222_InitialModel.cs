namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisciplineID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        ClassRoomId = c.Int(nullable: false),
                        WeekdayId = c.Int(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId, cascadeDelete: true)
                .ForeignKey("dbo.WeekDays", t => t.WeekdayId, cascadeDelete: true)
                .Index(t => t.ClassRoomId)
                .Index(t => t.WeekdayId);
            
            CreateTable(
                "dbo.ClassRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        classroomId = c.String(maxLength: 10),
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
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MyProperty = c.String(nullable: false, maxLength: 50),
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
                        CredentialId = c.Int(nullable: false),
                        Credentials_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCredentials", t => t.Credentials_Id)
                .Index(t => t.Credentials_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Credentials_Id", "dbo.UserCredentials");
            DropForeignKey("dbo.Classes", "WeekdayId", "dbo.WeekDays");
            DropForeignKey("dbo.Classes", "ClassRoomId", "dbo.ClassRooms");
            DropIndex("dbo.Users", new[] { "Credentials_Id" });
            DropIndex("dbo.Classes", new[] { "WeekdayId" });
            DropIndex("dbo.Classes", new[] { "ClassRoomId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserCredentials");
            DropTable("dbo.Disciplines");
            DropTable("dbo.WeekDays");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.Classes");
        }
    }
}
