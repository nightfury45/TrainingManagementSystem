using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrainingManagementSystem.Models
{
    public class CourseTrainer
    {
        [Key]
        [ForeignKey("Course")]
        [Column(Order = 1)]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [Key]
        [ForeignKey("Trainer")]
        [Column(Order = 2)]
        public string TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}