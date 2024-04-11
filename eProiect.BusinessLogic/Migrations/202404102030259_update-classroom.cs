namespace eProiect.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateclassroom : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Classes", name: "Classoom_Id", newName: "Classroom_Id");
            RenameIndex(table: "dbo.Classes", name: "IX_Classoom_Id", newName: "IX_Classroom_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Classes", name: "IX_Classroom_Id", newName: "IX_Classoom_Id");
            RenameColumn(table: "dbo.Classes", name: "Classroom_Id", newName: "Classoom_Id");
        }
    }
}
