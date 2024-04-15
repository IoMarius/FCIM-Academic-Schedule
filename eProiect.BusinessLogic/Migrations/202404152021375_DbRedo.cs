﻿namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbRedo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserDisciplineId = c.Int(nullable: false),
                        AcademicGroupId = c.Int(nullable: false),
                        ClassRoomId = c.Int(nullable: false),
                        WeekDayId = c.Int(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicGroups", t => t.AcademicGroupId, cascadeDelete: true)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId, cascadeDelete: true)
                .ForeignKey("dbo.UserDisciplines", t => t.UserDisciplineId, cascadeDelete: true)
                .ForeignKey("dbo.WeekDays", t => t.WeekDayId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.UserDisciplineId)
                .Index(t => t.AcademicGroupId)
                .Index(t => t.ClassRoomId)
                .Index(t => t.WeekDayId);
            
            CreateTable(
                "dbo.ClassRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassroomName = c.String(maxLength: 10),
                        Floor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "dbo.UserDisciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DisciplineId = c.Int(nullable: false),
                        ClassTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .ForeignKey("dbo.ClassTypes", t => t.ClassTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DisciplineId)
                .Index(t => t.ClassTypeId);
            
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
                        TypeName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id, unique: true);
            
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
                        UserCredentialId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCredentials", t => t.UserCredentialId, cascadeDelete: true)
                .Index(t => t.UserCredentialId);
            
            CreateTable(
                "dbo.UserCredentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true);
            
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
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 30),
                        CookieString = c.String(nullable: false),
                        ExpireTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcademicGroupId = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicGroups", t => t.AcademicGroupId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.AcademicGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "AcademicGroupId", "dbo.AcademicGroups");
            DropForeignKey("dbo.Classes", "WeekDayId", "dbo.WeekDays");
            DropForeignKey("dbo.Classes", "UserDisciplineId", "dbo.UserDisciplines");
            DropForeignKey("dbo.UserDisciplines", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserCredentialId", "dbo.UserCredentials");
            DropForeignKey("dbo.UserDisciplines", "ClassTypeId", "dbo.ClassTypes");
            DropForeignKey("dbo.UserDisciplines", "DisciplineId", "dbo.Disciplines");
            DropForeignKey("dbo.Classes", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.Classes", "AcademicGroupId", "dbo.AcademicGroups");
            DropIndex("dbo.Students", new[] { "AcademicGroupId" });
            DropIndex("dbo.Students", new[] { "Id" });
            DropIndex("dbo.Sessions", new[] { "Id" });
            DropIndex("dbo.UserCredentials", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserCredentialId" });
            DropIndex("dbo.ClassTypes", new[] { "Id" });
            DropIndex("dbo.UserDisciplines", new[] { "ClassTypeId" });
            DropIndex("dbo.UserDisciplines", new[] { "DisciplineId" });
            DropIndex("dbo.UserDisciplines", new[] { "UserId" });
            DropIndex("dbo.ClassRooms", new[] { "Id" });
            DropIndex("dbo.Classes", new[] { "WeekDayId" });
            DropIndex("dbo.Classes", new[] { "ClassRoomId" });
            DropIndex("dbo.Classes", new[] { "AcademicGroupId" });
            DropIndex("dbo.Classes", new[] { "UserDisciplineId" });
            DropIndex("dbo.Classes", new[] { "Id" });
            DropTable("dbo.Students");
            DropTable("dbo.Sessions");
            DropTable("dbo.WeekDays");
            DropTable("dbo.UserCredentials");
            DropTable("dbo.Users");
            DropTable("dbo.ClassTypes");
            DropTable("dbo.Disciplines");
            DropTable("dbo.UserDisciplines");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.Classes");
            DropTable("dbo.AcademicGroups");
        }
    }
}
