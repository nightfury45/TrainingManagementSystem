namespace TrainingManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableTrainers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        Type = c.String(),
                        WorkPlace = c.String(),
                        Phone = c.Int(nullable: false),
                        Education = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Trainers", new[] { "UserId" });
            DropTable("dbo.Trainers");
        }
    }
}
