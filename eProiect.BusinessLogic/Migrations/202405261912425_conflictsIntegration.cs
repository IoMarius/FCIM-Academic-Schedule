namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conflictsIntegration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ConflictingClasses", name: "ConflictingWithId", newName: "ConflictingWith_Id");
            RenameColumn(table: "dbo.ConflictingClasses", name: "MainClassId", newName: "MainClass_Id");
            RenameIndex(table: "dbo.ConflictingClasses", name: "IX_ConflictingWithId", newName: "IX_ConflictingWith_Id");
            RenameIndex(table: "dbo.ConflictingClasses", name: "IX_MainClassId", newName: "IX_MainClass_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ConflictingClasses", name: "IX_MainClass_Id", newName: "IX_MainClassId");
            RenameIndex(table: "dbo.ConflictingClasses", name: "IX_ConflictingWith_Id", newName: "IX_ConflictingWithId");
            RenameColumn(table: "dbo.ConflictingClasses", name: "MainClass_Id", newName: "MainClassId");
            RenameColumn(table: "dbo.ConflictingClasses", name: "ConflictingWith_Id", newName: "ConflictingWithId");
        }
    }
}
