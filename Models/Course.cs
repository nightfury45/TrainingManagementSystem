using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TrainingManagementSystem.Attribute;

namespace TrainingManagementSystem.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        [Unique(ErrorMessage = "This Category already exists !!")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}