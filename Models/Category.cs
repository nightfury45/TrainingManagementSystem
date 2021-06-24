using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrainingManagementSystem.Attribute;

namespace TrainingManagementSystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [Unique(ErrorMessage = "This Course already exists !!")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}