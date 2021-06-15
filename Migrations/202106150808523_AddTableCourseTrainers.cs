namespace TrainingManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableCourseTrainers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseTrainers",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.TrainerId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.TrainerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseTrainers", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.CourseTrainers", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTrainers", new[] { "TrainerId" });
            DropIndex("dbo.CourseTrainers", new[] { "CourseId" });
            DropTable("dbo.CourseTrainers");
        }
    }
}
