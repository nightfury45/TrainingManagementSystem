namespace TrainingManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableCourseTrainees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseTrainees",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        TraineeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.TraineeId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Trainees", t => t.TraineeId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.TraineeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseTrainees", "TraineeId", "dbo.Trainees");
            DropForeignKey("dbo.CourseTrainees", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTrainees", new[] { "TraineeId" });
            DropIndex("dbo.CourseTrainees", new[] { "CourseId" });
            DropTable("dbo.CourseTrainees");
        }
    }
}
