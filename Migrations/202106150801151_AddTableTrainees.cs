namespace TrainingManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableTrainees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainees",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        Age = c.Int(nullable: false),
                        DoB = c.DateTime(nullable: false),
                        Education = c.String(),
                        ProgrammingLanguage = c.String(),
                        TOEICScore = c.Int(nullable: false),
                        ExperienceDetail = c.String(),
                        Department = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainees", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Trainees", new[] { "UserId" });
            DropTable("dbo.Trainees");
        }
    }
}
