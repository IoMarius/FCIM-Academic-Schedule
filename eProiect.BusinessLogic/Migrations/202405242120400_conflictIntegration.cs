namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conflictIntegration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConflictingClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConflictId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.ClassId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConflictingClasses", "ClassId", "dbo.Classes");
            DropIndex("dbo.ConflictingClasses", new[] { "ClassId" });
            DropIndex("dbo.ConflictingClasses", new[] { "Id" });
            DropTable("dbo.ConflictingClasses");
        }
    }
}
