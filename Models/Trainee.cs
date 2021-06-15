using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrainingManagementSystem.Models
{
    public class Trainee
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime DoB { get; set; }
        public string Education { get; set; }
        public string ProgrammingLanguage { get; set; }
        public int TOEICScore { get; set; }
        public string ExperienceDetail { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }

    }
}